type Child = Node | string | null | undefined;
type Props<K extends keyof HTMLElementTagNameMap> = Partial<Omit<HTMLElementTagNameMap[K], 'style'>> & {
  style?: string;
};

export function h<K extends keyof HTMLElementTagNameMap>(
  tag: K,
  props: Props<K> = {},
  ...children: Child[]
): HTMLElementTagNameMap[K] {
  const { style, ...rest } = props;
  const element = Object.assign(document.createElement(tag), rest);
  if (style) {
    element.style.cssText = style;
  }
  element.append(...children.filter((c): c is Node | string => c != null));
  return element;
}
