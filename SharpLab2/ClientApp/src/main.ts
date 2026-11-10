import { h } from './dom';
import { entities } from './entities';
import { renderForm, renderHome, renderList } from './views';

const app = document.getElementById('app')!;

const links = [
  { route: '', title: 'Головна' },
  ...entities.map(e => ({ route: e.meta.route, title: e.meta.title }))
];

const navLinks = links.map(l =>
  h('li', { className: 'nav-item' }, h('a', { className: 'nav-link text-dark', href: `#/${l.route}` }, l.title))
);

const nav = h(
  'header',
  {},
  h(
    'nav',
    { className: 'navbar navbar-expand-sm navbar-light bg-light border-bottom mb-4' },
    h(
      'div',
      { className: 'container-fluid px-md-5 px-3' },
      h('a', { className: 'navbar-brand fw-semibold', href: '#/' }, 'Резервування квитків'),
      h('ul', { className: 'navbar-nav flex-grow-1' }, ...navLinks),
      h(
        'ul',
        { className: 'navbar-nav' },
        h(
          'li',
          { className: 'nav-item dropdown' },
          h(
            'a',
            { className: 'nav-link dropdown-toggle text-primary fw-semibold', href: '#', role: 'button' },
            'Web API (JSON)'
          ),
          h(
            'ul',
            { className: 'dropdown-menu dropdown-menu-end' },
            ...entities.map(e =>
              h(
                'li',
                {},
                h('a', { className: 'dropdown-item', href: `/api/${e.meta.route}`, target: '_blank' }, `/api/${e.meta.route}`)
              )
            )
          )
        )
      )
    )
  )
);

nav.querySelector('.dropdown-toggle')!.setAttribute('data-bs-toggle', 'dropdown');

const content = h('main', { className: 'pb-4', role: 'main' });

app.append(
  nav,
  h('div', { className: 'container-fluid px-md-5 px-3 flex-grow-1' }, content),
  h(
    'footer',
    { className: 'border-top footer text-muted py-3 mt-auto bg-light' },
    h(
      'div',
      { className: 'container-fluid px-md-5 px-3' },
      '© 2026 - Лабораторна робота №8 - ASP.NET Core Web API, TypeScript & Entity Framework Core'
    )
  )
);

async function navigate(): Promise<void> {
  const [route, part] = location.hash.replace(/^#\//, '').split('/');
  const entity = entities.find(e => e.meta.route === route);

  navLinks.forEach(item => {
    const link = item.querySelector('a')!;
    link.classList.toggle('fw-bold', link.hash === `#/${route ?? ''}`);
  });

  if (!entity) {
    content.replaceChildren(await renderHome(entities));
    return;
  }

  const view =
    part === undefined
      ? await renderList(entity)
      : await renderForm(entity, part === 'new' ? undefined : Number(part));

  content.replaceChildren(view);
}

window.addEventListener('hashchange', navigate);
navigate();
