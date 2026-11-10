export function formatDate(iso: string): string {
  const [year, month, day] = iso.split('-');
  return year && month && day ? `${day}.${month}.${year}` : iso;
}

export function formatMoney(value: number): string {
  return `${value.toFixed(2)} грн`;
}

export function today(): string {
  return new Date().toISOString().slice(0, 10);
}
