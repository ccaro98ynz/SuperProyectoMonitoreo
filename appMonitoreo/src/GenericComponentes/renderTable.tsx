interface Props<T> {
    data: T[];
    tableId: string;
}

export default function RenderTable<T extends object>({ data, tableId }: Props<T>) {
    if (!data.length) return <p>Sin registros</p>;

    return (
        <table id={tableId} border={1}>
            <thead>
                <tr>
                    {Object.keys(data[0]).map(key => <th key={key}>{key}</th>)}
                </tr>
            </thead>
            <tbody>
                {data.map((item, i) => (
                    <tr key={i}>
                        {Object.values(item).map((value, j) =>
                            <td key={j}>{String(value ?? '')}</td>
                        )}
                    </tr>
                ))}
            </tbody>
        </table>
    );
}