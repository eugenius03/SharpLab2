import type { Identifiable } from './types';

export class ApiError extends Error {
  constructor(public readonly messages: string[]) {
    super(messages.join('\n'));
  }
}

async function request<T>(url: string, init?: RequestInit): Promise<T> {
  const response = await fetch(url, {
    ...init,
    headers: { 'Content-Type': 'application/json' }
  });

  if (response.status === 404) {
    throw new ApiError(['Запис не знайдено.']);
  }

  if (response.status === 400) {
    const problem = await response.json();
    const messages = Object.values<string[]>(problem.errors ?? {}).flat();
    throw new ApiError(messages.length > 0 ? messages : ['Некоректні дані.']);
  }

  if (!response.ok) {
    throw new ApiError([`Помилка сервера (${response.status}).`]);
  }

  return response.status === 204 ? (undefined as T) : response.json();
}

export class CrudApi<TResponse extends Identifiable, TRequest> {
  constructor(private readonly endpoint: string) {}

  getAll(): Promise<TResponse[]> {
    return request(this.endpoint);
  }

  getById(id: number): Promise<TResponse> {
    return request(`${this.endpoint}/${id}`);
  }

  create(body: TRequest): Promise<TResponse> {
    return request(this.endpoint, { method: 'POST', body: JSON.stringify(body) });
  }

  update(id: number, body: TRequest): Promise<void> {
    return request(`${this.endpoint}/${id}`, { method: 'PUT', body: JSON.stringify(body) });
  }

  remove(id: number): Promise<void> {
    return request(`${this.endpoint}/${id}`, { method: 'DELETE' });
  }
}
