export interface IListagemDeclaracaoEntregues {
  tipo: string;
  situacao: string;
  indicadorPagamentoReembolso: string;
  baseTributavelCentimos: number;
  impostoLiquidadoCentimos: number;
  impostoDedutivelCentimos: number;
  valor1: number;
  valor2: number;
  periodo: string;
  dataRececao: string;
  numeroDeclaracao: string;
}
