import { ApiError, CrudApi } from './api';
import type { Identifiable } from './types';

export interface Option {
  id: number;
  text: string;
}

export type FieldDefinition<TRequest> = {
  key: keyof TRequest & string;
  label: string;
} & (
  | { kind: 'text' | 'number' | 'date' }
  | { kind: 'select'; options: () => Promise<Option[]> }
);

export interface FieldView {
  key: string;
  label: string;
  kind: 'text' | 'number' | 'date' | 'select';
  options: Option[];
  value: string;
}

export interface EntityMeta {
  route: string;
  title: string;
  listTitle: string;
  description: string;
  addLabel: string;
  singular: string;
}

export interface ColumnDefinition<TResponse> {
  header: string;
  cell: (item: TResponse) => string;
  className?: string;
}

export interface Row {
  id: number;
  cells: string[];
}

export interface EntityModule {
  meta: EntityMeta;
  columns: { header: string; className?: string }[];
  load(): Promise<Row[]>;
  remove(id: number): Promise<void>;
  openForm(id?: number): Promise<FieldView[]>;
  save(id: number | undefined, values: Record<string, string>): Promise<void>;
}

export class Entity<TResponse extends Identifiable, TRequest> implements EntityModule {
  readonly columns: { header: string; className?: string }[];

  constructor(
    readonly meta: EntityMeta,
    private readonly api: CrudApi<TResponse, TRequest>,
    private readonly columnDefinitions: ColumnDefinition<TResponse>[],
    private readonly fields: FieldDefinition<TRequest>[],
    private readonly createDefaults: () => TRequest,
    private readonly toRequest: (item: TResponse) => TRequest
  ) {
    this.columns = columnDefinitions.map(({ header, className }) => ({ header, className }));
  }

  async load(): Promise<Row[]> {
    const items = await this.api.getAll();
    return items.map(item => ({
      id: item.id,
      cells: this.columnDefinitions.map(column => column.cell(item))
    }));
  }

  remove(id: number): Promise<void> {
    return this.api.remove(id);
  }

  async openForm(id?: number): Promise<FieldView[]> {
    const request = id === undefined ? this.createDefaults() : this.toRequest(await this.api.getById(id));

    return Promise.all(
      this.fields.map(async field => ({
        key: field.key,
        label: field.label,
        kind: field.kind,
        options: field.kind === 'select' ? await field.options() : [],
        value: String(request[field.key])
      }))
    );
  }

  async save(id: number | undefined, values: Record<string, string>): Promise<void> {
    const errors: string[] = [];
    const body: Record<string, string | number> = {};

    for (const field of this.fields) {
      const raw = values[field.key].trim();
      if (field.kind === 'number' || field.kind === 'select') {
        const number = Number(raw.replace(',', '.'));
        if (raw === '' || Number.isNaN(number)) {
          errors.push(`Поле «${field.label}» повинно бути числом.`);
        }
        body[field.key] = number;
      } else {
        body[field.key] = raw;
      }
    }

    if (errors.length > 0) {
      throw new ApiError(errors);
    }

    if (id === undefined) {
      await this.api.create(body as TRequest);
    } else {
      await this.api.update(id, body as TRequest);
    }
  }
}
