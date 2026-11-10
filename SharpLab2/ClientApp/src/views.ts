import { ApiError } from './api';
import { h } from './dom';
import type { EntityModule, FieldView, Row } from './entity';

function errorAlert(): HTMLElement {
  return h('div', { className: 'alert alert-danger', hidden: true });
}

function showErrors(box: HTMLElement, error: unknown): void {
  const messages = error instanceof ApiError ? error.messages : ['Не вдалося виконати запит.'];
  box.replaceChildren(...messages.map(m => h('div', {}, m)));
  box.hidden = false;
}

export async function renderHome(entities: EntityModule[]): Promise<HTMLElement> {
  const counts = await Promise.all(entities.map(async e => (await e.load()).length));

  return h(
    'section',
    {},
    h(
      'div',
      { className: 'text-center my-4' },
      h('h1', { className: 'display-5' }, 'Резервування квитків'),
      h('p', { className: 'lead' }, 'Лабораторна робота №8: ASP.NET Core Web API, TypeScript та Entity Framework Core.')
    ),
    h('hr'),
    h(
      'div',
      { className: 'row mt-4' },
      h(
        'div',
        { className: 'col-md-6 mb-4' },
        h('h4', {}, 'Статистика системи'),
        h(
          'ul',
          { className: 'list-group' },
          ...entities.map((e, i) =>
            h(
              'li',
              { className: 'list-group-item d-flex justify-content-between align-items-center' },
              h('a', { href: `#/${e.meta.route}`, className: 'text-decoration-none' }, e.meta.title),
              h('span', { className: 'badge bg-primary rounded-pill' }, String(counts[i]))
            )
          )
        )
      )
    )
  );
}

export async function renderList(entity: EntityModule): Promise<HTMLElement> {
  const { meta } = entity;
  const error = errorAlert();
  const tableBody = h('tbody');
  const total = h('div', { className: 'col-md-6 text-md-end text-muted' });
  const modalHost = h('div');
  const search = h('input', { type: 'text', className: 'form-control', placeholder: 'Пошук...' });
  let rows: Row[] = [];

  const openDeleteModal = (row: Row) => {
    const close = () => modalHost.replaceChildren();
    modalHost.replaceChildren(
      h(
        'div',
        { className: 'modal fade show d-block', style: 'background-color: rgba(0,0,0,0.5);' },
        h(
          'div',
          { className: 'modal-dialog' },
          h(
            'div',
            { className: 'modal-content' },
            h(
              'div',
              { className: 'modal-header' },
              h('h5', { className: 'modal-title text-danger' }, 'Підтвердження видалення'),
              h('button', { type: 'button', className: 'btn-close', onclick: close })
            ),
            h(
              'div',
              { className: 'modal-body' },
              h('p', { className: 'mb-0' }, `Ви дійсно бажаєте видалити запис №${row.id}?`)
            ),
            h(
              'div',
              { className: 'modal-footer' },
              h('button', { type: 'button', className: 'btn btn-secondary', onclick: close }, 'Скасувати'),
              h(
                'button',
                {
                  type: 'button',
                  className: 'btn btn-danger',
                  onclick: async () => {
                    close();
                    try {
                      await entity.remove(row.id);
                    } catch (e) {
                      showErrors(error, e);
                    }
                    await reload();
                  }
                },
                'Видалити'
              )
            )
          )
        )
      )
    );
  };

  const draw = () => {
    const term = search.value.trim().toLowerCase();
    const visible = rows.filter(r => r.cells.join(' ').toLowerCase().includes(term));
    total.textContent = `Всього записів: ${visible.length}`;
    tableBody.replaceChildren(
      ...visible.map(row =>
        h(
          'tr',
          {},
          h('td', { className: 'fw-bold' }, String(row.id)),
          ...row.cells.map((cell, i) => h('td', { className: entity.columns[i].className ?? '' }, cell)),
          h(
            'td',
            { className: 'text-end text-nowrap' },
            h(
              'a',
              { className: 'btn btn-sm btn-outline-secondary me-1', href: `#/${meta.route}/${row.id}` },
              'Редагувати'
            ),
            h(
              'button',
              { className: 'btn btn-sm btn-outline-danger', onclick: () => openDeleteModal(row) },
              'Видалити'
            )
          )
        )
      )
    );
  };

  const reload = async () => {
    try {
      rows = await entity.load();
      draw();
    } catch (e) {
      showErrors(error, e);
    }
  };

  search.oninput = draw;
  await reload();

  return h(
    'section',
    {},
    h(
      'div',
      { className: 'd-flex justify-content-between align-items-center mb-3' },
      h('div', {}, h('h2', { className: 'mb-1' }, meta.listTitle), h('p', { className: 'text-muted mb-0' }, meta.description)),
      h('a', { className: 'btn btn-primary', href: `#/${meta.route}/new` }, meta.addLabel)
    ),
    h(
      'div',
      { className: 'row g-2 align-items-center mb-3' },
      h('div', { className: 'col-md-6' }, search),
      total
    ),
    error,
    h(
      'div',
      { className: 'table-responsive' },
      h(
        'table',
        { className: 'table table-striped table-hover align-middle border' },
        h(
          'thead',
          { className: 'table-light' },
          h(
            'tr',
            {},
            h('th', { style: 'width: 50px;' }, '#'),
            ...entity.columns.map(c => h('th', { className: c.className?.includes('text-center') ? 'text-center' : '' }, c.header)),
            h('th', { className: 'text-end', style: 'width: 220px;' }, 'Дії')
          )
        ),
        tableBody
      )
    ),
    modalHost
  );
}

export async function renderForm(entity: EntityModule, id?: number): Promise<HTMLElement> {
  const { meta } = entity;
  const fields = await entity.openForm(id);
  const error = errorAlert();
  const inputs = new Map<string, HTMLInputElement | HTMLSelectElement>();
  const creating = id === undefined;

  const form = h(
    'form',
    {
      onsubmit: async (event: SubmitEvent) => {
        event.preventDefault();
        const values = Object.fromEntries([...inputs].map(([key, input]) => [key, input.value]));
        try {
          await entity.save(id, values);
          location.hash = `#/${meta.route}`;
        } catch (e) {
          showErrors(error, e);
        }
      }
    },
    error,
    ...fields.map(field => {
      const input = createInput(field);
      inputs.set(field.key, input);
      return h('div', { className: 'mb-3' }, h('label', { className: 'form-label fw-bold' }, field.label), input);
    }),
    h(
      'div',
      { className: 'd-flex justify-content-between mt-4' },
      h('a', { className: 'btn btn-outline-secondary', href: `#/${meta.route}` }, 'Назад до списку'),
      h('button', { type: 'submit', className: 'btn btn-success px-4' }, 'Зберегти')
    )
  );

  return h(
    'div',
    { className: 'row justify-content-center' },
    h(
      'div',
      { className: 'col-lg-7' },
      h(
        'div',
        { className: 'card shadow-sm' },
        h(
          'div',
          { className: `card-header ${creating ? 'bg-primary text-white' : 'bg-warning text-dark'}` },
          h('h4', { className: 'mb-0' }, creating ? `Додавання ${meta.singular}` : `Редагування ${meta.singular} №${id}`)
        ),
        h('div', { className: 'card-body p-4' }, form)
      )
    )
  );
}

function createInput(field: FieldView): HTMLInputElement | HTMLSelectElement {
  if (field.kind === 'select') {
    const select = h(
      'select',
      { className: 'form-select' },
      h('option', { value: '0' }, '-- Оберіть зі списку --'),
      ...field.options.map(o => h('option', { value: String(o.id) }, o.text))
    );
    select.value = field.value;
    return select;
  }

  return h('input', {
    type: field.kind === 'date' ? 'date' : 'text',
    className: 'form-control',
    value: field.value,
    inputMode: field.kind === 'number' ? 'decimal' : 'text'
  });
}
