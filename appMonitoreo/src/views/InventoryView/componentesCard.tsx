
import { useEffect, useState } from 'react';
import {
    fetchComponentes,
    fetchComponentebyId,
    createComponente,
    updateComponente,
    disableComponente
} from '../../services/componentes';

import type { consultaComponentes, newComponente } from '../../types/componentesDtos';
import RenderTable from '../../GenericComponentes/renderTable';

const formInicial = (): newComponente => ({
    Codigo: '',
    Nombre: '',
    Marca: '',
    IdCategoria: 1,
    UnidadMedida: '',
    Descripcion: '',
    StockMinimo: 0,
    StockMaximo: 0,
    Imagen: null
});

const ComponentesCard = () => {
    const [componentes, setComponentes] = useState<consultaComponentes[]>([]);
    const [form, setForm] = useState<newComponente>(formInicial);
    const [idEditar, setIdEditar] = useState<number | null>(null);
    const [error, setError] = useState('');
    const [cargando, setCargando] = useState(false);

    const cargar = async () => setComponentes(await fetchComponentes());

    useEffect(() => {
        fetchComponentes().then(setComponentes).catch(e => setError(String(e)));
    }, []);

    const cambiar = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value, files } = e.target;

        setForm(prev => ({
            ...prev,
            [name]: name === 'Imagen'
                ? files?.[0] ?? null
                : ['IdCategoria', 'StockMinimo', 'StockMaximo'].includes(name)
                    ? Number(value) : value
        }));
    };

    const limpiar = () => {
        setForm(formInicial());
        setIdEditar(null);
        setError('');
    };

    const editar = async (id: number) => {
        try {
            const c = await fetchComponentebyId(id);

            setForm({
                Codigo: c.codigo,
                Nombre: c.nombre,
                Marca: c.marca ?? '',
                IdCategoria: c.idCategoria,
                UnidadMedida: c.unidadMedida ?? '',
                Descripcion: c.descripcion ?? '',
                StockMinimo: c.stockMinimo ?? 0,
                StockMaximo: c.stockMaximo ?? 0,
                Imagen: null
            });

            setIdEditar(id);
            setError('');
        } catch (e) {
            setError(String(e));
        }
    };

    const guardar = async () => {
        if (!form.Codigo.trim() || !form.Nombre.trim())
            return setError('Código y nombre obligatorios.');

        if (form.StockMaximo < form.StockMinimo)
            return setError('El stock máximo no puede ser menor al mínimo.');

        setCargando(true);
        setError('');

        try {
            if (idEditar === null) await createComponente(form);
            else await updateComponente(idEditar, form);

            await cargar();
            limpiar();
        } catch (e) {
            setError(String(e));
        } finally {
            setCargando(false);
        }
    };

    const desactivar = async (id: number) => {
        if (!confirm('¿Desactivar componente?')) return;

        try {
            await disableComponente(id);
            await cargar();
            if (idEditar === id) limpiar();
        } catch (e) {
            setError(String(e));
        }
    };

    return (
        <section>
            <h2>Administración de componentes</h2>

            <h3>{idEditar === null ? 'Registrar componente' : 'Editar componente'}</h3>

            <form onSubmit={e => { e.preventDefault(); void guardar(); }}>
                <p>
                    <input name="Codigo" placeholder="Código"
                        value={form.Codigo} onChange={cambiar} required />
                </p>
                <p>
                    <input name="Nombre" placeholder="Nombre"
                        value={form.Nombre} onChange={cambiar} required />
                </p>
                <p>
                    <input name="Marca" placeholder="Marca"
                        value={form.Marca} onChange={cambiar} />
                </p>
                <p>
                    <label>ID Categoría: </label>
                    <input name="IdCategoria" type="number" min="1"
                        value={form.IdCategoria} onChange={cambiar} required />
                </p>
                <p>
                    <input name="UnidadMedida" placeholder="Unidad de medida"
                        value={form.UnidadMedida} onChange={cambiar} />
                </p>
                <p>
                    <input name="Descripcion" placeholder="Descripción"
                        value={form.Descripcion} onChange={cambiar} />
                </p>
                <p>
                    <label>Stock mínimo: </label>
                    <input name="StockMinimo" type="number" min="0" step="0.001"
                        value={form.StockMinimo} onChange={cambiar} />
                </p>
                <p>
                    <label>Stock máximo: </label>
                    <input name="StockMaximo" type="number" min="0" step="0.001"
                        value={form.StockMaximo} onChange={cambiar} />
                </p>
                <p>
                    <label>Imagen: </label>
                    <input key={idEditar ?? 'nuevo'} name="Imagen" type="file"
                        accept="image/jpeg,image/png,image/webp" onChange={cambiar} />
                </p>

                <button type="submit" disabled={cargando}>
                    {idEditar === null ? 'Registrar' : 'Actualizar'}
                </button>

                <button type="button" onClick={limpiar}>Cancelar</button>
            </form>

            {error && <p role="alert">{error}</p>}

            <hr />

            <h3>Componentes registrados</h3>

            <RenderTable data={componentes} tableId="tabla-componentes" />

            <h3>Acciones</h3>

            <ul>
                {componentes.map(c => (
                    <li key={c.idComponente}>
                        {c.codigo} - {c.nombre}{' '}
                        <button type="button" onClick={() => editar(c.idComponente)}>
                            Editar
                        </button>{' '}
                        <button type="button" onClick={() => desactivar(c.idComponente)}>
                            Desactivar
                        </button>
                    </li>
                ))}
            </ul>
        </section>
    );
};

export default ComponentesCard;
