export interface newComponente {
    Codigo: string;
    Nombre: string;
    Marca: string;
    IdCategoria: number;
    UnidadMedida: string;
    Descripcion: string;
    StockMinimo: number;
    StockMaximo: number;
    Imagen: File | null;
}
export interface consultaComponentes {
    idComponente: number;
    codigo: string;
    nombre: string;
    marca: string | null;
    descripcion: string | null;
    categoria: string;
    idCategoria: number;
    unidadMedida: string | null;
    stockMinimo: number | null;
    stockMaximo: number | null;
    imagenUrl: string | null;
}

