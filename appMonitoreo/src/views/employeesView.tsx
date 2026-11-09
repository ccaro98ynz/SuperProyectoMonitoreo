import {SignUp, Login} from '../services/employees';

const EmployeesView = () => {
    const empleado = {
        Nombre: 'John',
        Apellido: 'Doe',
        Email: 'emailprueba123@gmail.com',
        Password : 'password123'
    }
    const credenciales = {
        Email: 'emailprueba123@gmail.com',
        Password: 'password123'
    }
    return (
        <div>
            <h1>Employees</h1>
            <button 
            onClick={() => SignUp(empleado)}>
            Sign Up</button>
            <button 
            onClick={() => Login(credenciales)}>
            Login</button>
        </div>
    );
};
export default EmployeesView;