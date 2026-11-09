import {apiRequest} from './apiRequest';
import type{  Credenciales, Empleado } from '../types/employeesDtos';
const employeesUrl = '/employees';

export async function SignUp(empleado: Empleado) {
    return await  apiRequest(`${employeesUrl}/sign-up`, {
        method: 'POST',
        body: JSON.stringify(empleado)
    });
}

export async function Login(credenciales: Credenciales) {
    return await apiRequest(`${employeesUrl}/login`, {
        method: 'POST',
        body: JSON.stringify(credenciales)
    });
}
