import {
    apiRequest,
    toFormData
} from './apiRequest';
import type{  newComponente,  consultaComponentes } from '../types/componentesDtos';
const componentesUrl = '/componentes';

export async function fetchComponentes():Promise<consultaComponentes[]> {
    return await  apiRequest<consultaComponentes[]>(`${componentesUrl}/all`);
}
export const fetchComponentebyId = (id:number) => 
apiRequest<consultaComponentes>(`${componentesUrl}/view/${id}`);

export async function createComponente(componente: newComponente) {
    return await  apiRequest(`${componentesUrl}/create`, {
        method: 'POST',
        body: toFormData(componente)
    });
}
export async function updateComponente(id:number, componente: newComponente) {
    return await  apiRequest(`${componentesUrl}/update/${id}`, {
        method: 'PUT',
        body: toFormData(componente)
    });
}
export const disableComponente = (id:number) =>   
    apiRequest(`${componentesUrl}/disable/${id}`, {
        method: 'PATCH'
    });
