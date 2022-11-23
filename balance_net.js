(function execute(inputs, outputs) {
  var dailyAllocationConsolidation = inputs.dailyAllocationConsolidation;
  var responseDate = inputs.responseDate;
  var volumesPerUee = [];
  var comments = [];

  var gaPIData = new GlideAggregate("x_petro_sie_pi_data");
  gaPIData.addQuery("destination", dailyAllocationConsolidation.sys_id);
  gaPIData.addAggregate("SUM", "value");
  gaPIData.groupBy("origin");
  gaPIData.groupBy("tag.type");
  gaPIData.query();
  while (gaPIData.next()) {
    var origin = gaPIData.origin;
    var origin_table = gaPIData.origin_table;
    var type = gaPIData.getValue("tag.type");
    var sum = gaPIData.getAggregate("SUM", "value");

    if (origin_table == "x_petro_sie_process_parameter") {
      var newImpNetBalancesPP = new GlideRecord(
        "x_petro_sie_import_net_balance"
      );
      newImpNetBalancesPP.initialize;
      newImpNetBalancesPP.daily_allocation_consolidated =
        dailyAllocationConsolidation.sys_id;
      newImpNetBalancesPP.operating_day = responseDate;
      newImpNetBalancesPP.origin = origin;
      newImpNetBalancesPP.origin_table = origin_table;
      newImpNetBalancesPP.exported_value = "0";
      newImpNetBalancesPP.imported_value = sum;
      newImpNetBalancesPP.gross_balance = sum;
      newImpNetBalancesPP.net_balance = sum;
      newImpNetBalancesPP.insert();
    }

    if (origin_table == "x_petro_sie_uee") {
      if (!volumesPerUee[origin.sys_id]) {
        volumesPerUee[origin.sys_id] = {
          ImportVolume: 0,
          ExportVolume: 0,
        };
      }

      if (type == "uee_export") {
        volumesPerUee[origin.sys_id].ExportVolume += sum;

        var newImpNetBalancesUEE = new GlideRecord(
          "x_petro_sie_import_net_balance"
        );
        newImpNetBalancesUEE.initialize;
        newImpNetBalancesUEE.daily_allocation_consolidated =
          dailyAllocationConsolidation.sys_id;
        newImpNetBalancesUEE.operating_day = responseDate;
        newImpNetBalancesUEE.origin = origin;
        newImpNetBalancesUEE.origin_table = origin_table;
        newImpNetBalancesUEE.insert();
        
      } else if (type == "uee_import") {
        volumesPerUee[origin.sys_id].ImportVolume += sum;
      }
    }

    for (var uee in volumesPerUee) {
      var grossBalance =
        volumesPerUee[uee].ExportVolume - volumesPerUee[uee].ImportVolume;

      var grImpNetBalancesUEE = new GlideRecord(
        "x_petro_sie_import_net_balance"
      );
      grImpNetBalancesUEE.addQuery("origin", uee);
      grImpNetBalancesUEE.addQuery(
        "daily_allocation_consolidated",
        dailyAllocationConsolidation
      );
      grImpNetBalancesUEE.query();
      while (grImpNetBalancesUEE.next()) {
        grImpNetBalancesUEE.uee = uee;
        grImpNetBalancesUEE.exported_value = volumesPerUee[uee].ExportVolume;
        grImpNetBalancesUEE.imported_value = volumesPerUee[uee].ImportVolume;
        grImpNetBalancesUEE.gross_balance = grossBalance;
        grImpNetBalancesUEE.net_balance = grossBalance;
        grImpNetBalancesUEE.update();
      }
    }    
  }

  var grImpNetBalances = new GlideRecord("x_petro_sie_import_net_balance");
    grImpNetBalances.addQuery(
      "daily_allocation_consolidated",
      dailyAllocationConsolidation
    );
    grImpNetBalances.addEncodedQuery("net_balance<0");
    grImpNetBalances.query();
    while (grImpNetBalances.next()) {
      var net_balanceImp = parseFloat(grImpNetBalances.net_balance) * - 1;
      var ueeNeedToImport = grImpNetBalances.uee;
      var grImpPriority = new GlideRecord("x_petro_sie_import_priority");
      grImpPriority.addQuery("uee_importer", ueeNeedToImport);
      grImpPriority.addQuery("order", "1");
      grImpPriority.query();
      if (grImpPriority.next()) {
        var ueeThatWillExport = grImpPriority.uee_exporter;
       	var ueeThatWillExportName = new GlideRecord('x_petro_sie_uee');
        ueeThatWillExportName.get(ueeThatWillExport)

        var ueeNeedToImportName = new GlideRecord('x_petro_sie_uee');
        ueeNeedToImportName.get(ueeNeedToImport)

        var grExpNetBalances = new GlideRecord(
          "x_petro_sie_import_net_balance"
        );
        grExpNetBalances.addQuery(
          "daily_allocation_consolidated",
          dailyAllocationConsolidation
        );
        grExpNetBalances.addQuery("uee", ueeThatWillExport);
        grExpNetBalances.addEncodedQuery("net_balance>0");
        grExpNetBalances.query();
        if (grExpNetBalances.next()) {
          var net_balanceExp = parseFloat(grExpNetBalances.net_balance);
          var balance = net_balanceExp - net_balanceImp;
          var balanceMoviments = net_balanceExp - balance;
          if (net_balanceExp > net_balanceImp) {
            grImpNetBalances.setValue("net_balance", "0");
            grImpNetBalances.movements = "Foi importado " + net_balanceImp + " da UEE " + ueeThatWillExportName.getValue('nome');
            grImpNetBalances.update();
            grExpNetBalances.setValue("net_balance", balance);
            grExpNetBalances.movements = "Foi exportado " + net_balanceImp + " para a UEE " + ueeNeedToImportName.getValue('nome');
            grExpNetBalances.update();
          } else {
            grImpNetBalances.setValue("net_balance", balance);
            grImpNetBalances.movements = "Foi importado " + net_balanceExp + " da UEE " + ueeThatWillExportName.getValue('nome');
            grImpNetBalances.update();
            grExpNetBalances.setValue("net_balance", "0");
            grExpNetBalances.movements = "Foi exportado " + net_balanceExp + " para a UEE " + ueeNeedToImportName.getValue('nome');
            grExpNetBalances.update();
          }
        }
      }
    }

    var grImpNetBalances = new GlideRecord("x_petro_sie_import_net_balance");
    grImpNetBalances.addQuery(
      "daily_allocation_consolidated",
      dailyAllocationConsolidation
    );
    grImpNetBalances.addEncodedQuery("net_balance<0");
    grImpNetBalances.query();
    while (grImpNetBalances.next()) {
      var net_balanceImp = parseFloat(grImpNetBalances.net_balance) * - 1;
      var ueeNeedToImport = grImpNetBalances.uee;
      var grImpPriority = new GlideRecord("x_petro_sie_import_priority");
      grImpPriority.addQuery("uee_importer", ueeNeedToImport);
      grImpPriority.addQuery("order", "2");
      grImpPriority.query();
      if (grImpPriority.next()) {
        var ueeThatWillExport = grImpPriority.uee_exporter;
       	var ueeThatWillExportName = new GlideRecord('x_petro_sie_uee');
        ueeThatWillExportName.get(ueeThatWillExport)

        var ueeNeedToImportName = new GlideRecord('x_petro_sie_uee');
        ueeNeedToImportName.get(ueeNeedToImport)

        var grExpNetBalances = new GlideRecord(
          "x_petro_sie_import_net_balance"
        );
        grExpNetBalances.addQuery(
          "daily_allocation_consolidated",
          dailyAllocationConsolidation
        );
        grExpNetBalances.addQuery("uee", ueeThatWillExport);
        grExpNetBalances.addEncodedQuery("net_balance>0");
        grExpNetBalances.query();
        if (grExpNetBalances.next()) {
          var net_balanceExp = parseFloat(grExpNetBalances.net_balance);
          var balance = net_balanceExp - net_balanceImp;
          var balanceMoviments = net_balanceExp - balance;
          if (net_balanceExp > net_balanceImp) {
            grImpNetBalances.setValue("net_balance", "0");
            grImpNetBalances.movements = "Foi importado " + net_balanceImp + " da UEE " + ueeThatWillExportName.getValue('nome');
            grImpNetBalances.update();
            grExpNetBalances.setValue("net_balance", balance);
            grExpNetBalances.movements = "Foi exportado " + net_balanceImp + " para a UEE " + ueeNeedToImportName.getValue('nome');
            grExpNetBalances.update();
          } else {
            grImpNetBalances.setValue("net_balance", balance);
            grImpNetBalances.movements = "Foi importado " + net_balanceExp + " da UEE " + ueeThatWillExportName.getValue('nome');
            grImpNetBalances.update();
            grExpNetBalances.setValue("net_balance", "0");
            grExpNetBalances.movements = "Foi exportado " + balanceMoviments + " para a UEE " + ueeNeedToImportName.getValue('nome');
            grExpNetBalances.update();
          }
        }
      }
    }

    var grImpNetBalances = new GlideRecord("x_petro_sie_import_net_balance");
    grImpNetBalances.addQuery(
      "daily_allocation_consolidated",
      dailyAllocationConsolidation
    );
    grImpNetBalances.addEncodedQuery("net_balance<0");
    grImpNetBalances.query();
    while (grImpNetBalances.next()) {
      var net_balanceImp = parseFloat(grImpNetBalances.net_balance) * - 1;
      var ueeNeedToImport = grImpNetBalances.uee;
      var grImpPriority = new GlideRecord("x_petro_sie_import_priority");
      grImpPriority.addQuery("uee_importer", ueeNeedToImport);
      grImpPriority.addQuery("order", "3");
      grImpPriority.query();
      if (grImpPriority.next()) {
        var ueeThatWillExport = grImpPriority.uee_exporter;
       	var ueeThatWillExportName = new GlideRecord('x_petro_sie_uee');
        ueeThatWillExportName.get(ueeThatWillExport)

        var ueeNeedToImportName = new GlideRecord('x_petro_sie_uee');
        ueeNeedToImportName.get(ueeNeedToImport)

        var grExpNetBalances = new GlideRecord(
          "x_petro_sie_import_net_balance"
        );
        grExpNetBalances.addQuery(
          "daily_allocation_consolidated",
          dailyAllocationConsolidation
        );
        grExpNetBalances.addQuery("uee", ueeThatWillExport);
        grExpNetBalances.addEncodedQuery("net_balance>0");
        grExpNetBalances.query();
        if (grExpNetBalances.next()) {
          var net_balanceExp = parseFloat(grExpNetBalances.net_balance);
          var balance = net_balanceExp - net_balanceImp;
          var balanceMoviments = net_balanceExp - balance;
          if (net_balanceExp > net_balanceImp) {
            grImpNetBalances.setValue("net_balance", "0");
            grImpNetBalances.movements = "Foi importado " + net_balanceImp + " da UEE " + ueeThatWillExportName.getValue('nome');
            grImpNetBalances.update();
            grExpNetBalances.setValue("net_balance", balance);
            grExpNetBalances.movements = "Foi exportado " + net_balanceImp + " para a UEE " + ueeNeedToImportName.getValue('nome');
            grExpNetBalances.update();
          } else {
            grImpNetBalances.setValue("net_balance", balance);
            grImpNetBalances.movements = "Foi importado " + net_balanceExp + " da UEE " + ueeThatWillExportName.getValue('nome');
            grImpNetBalances.update();
            grExpNetBalances.setValue("net_balance", "0");
            grExpNetBalances.movements = "Foi exportado " + balanceMoviments + " para a UEE " + ueeNeedToImportName.getValue('nome');
            grExpNetBalances.update();
          }
        }
      }
    }
})(inputs, outputs);
