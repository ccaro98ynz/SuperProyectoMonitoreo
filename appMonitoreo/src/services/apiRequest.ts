const baseUrl = 'http://localhost:5050/api';

export async function apiRequest<T = void>(endpoint: string, options: RequestInit = {}): Promise<T> {
    const headers = new Headers(options.headers);

    if (options.body instanceof FormData)
        headers.delete('Content-Type');
    else if (options.body)
        headers.set('Content-Type', 'application/json');

    const res = await fetch(`${baseUrl}${endpoint}`, { ...options, headers });
    const text = await res.text();

    if (!res.ok) throw new Error(text || `HTTP ${res.status}`);

    return (text ? JSON.parse(text) : undefined) as T;
}
export function toFormData(data: any): FormData {
    const form = new FormData();

    Object.entries(data).forEach(([key, value]) => {
        if (value !== null && value !== undefined)
            form.append(key, value instanceof File ? value : String(value));
    });

    return form;
}