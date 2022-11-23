# health-calc-pack-dotnet
Projeto apresentado para disciplina de Projeto Integrado de Engenharia de Software

## Pré-requisitos

.NET CORE SDK 6.0
https://dotnet.microsoft.com/en-us/download

XUnit
https://xunit.net/

## Como instalar
Para adicionar o pacote ao seu projeto:

`dotnet add package health-calc-pack-dotnet-sjmc`

## Conteúdo do pacote

1. Calculadora de IMC
2. Calculadora de Macronutrientes

## Como utilizar

1º passo: Informa-se a altura da pessoa a ter seus dados calculados;

2º passo: Informa-se o peso da mesma pessoa;

3º passo: Informa-se o sexo da pessoa.

A partir desses dados é calculada a classificação em que a pessoa se encontra no momento e a aplicação retorna além da classificação da pessoa calculada, uma indicação da uma alimentação balanceada em seus macronutrientes.

O indivíduo pode ser classificado nas seguintes categorias:

- Abaixo do peso
- Peso normal
- Pré-obesidade
- Obesidade Grau I
- Obesidade Grau II
- Obesidade Grau III

O cálculo de macronutrientes é realizada com base na estratégia sugerida

- Cutting <br/>
 2g de proteina por kg<br/>
 1g de gordura por kg<br/>
 4g de carboidrato por kg

- Bulking <br/>
 2g de proteina por kg<br/>
 2g de gordura por kg<br/>
 4g de carboidrato por kg

- Maintenance
<br/>
 2g de proteina por kg<br/>
 1g de gordura por kg<br/>
 5g de carboidrato por kg

## Exemplo 

<img src="img\exemplo.PNG" alt="exemplo">