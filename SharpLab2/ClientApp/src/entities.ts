import { CrudApi } from './api';
import { Entity, type EntityModule } from './entity';
import { formatDate, formatMoney, today } from './format';
import type {
  CarriageTypeRequest,
  CarriageTypeResponse,
  DestinationRequest,
  DestinationResponse,
  PassengerRequest,
  PassengerResponse,
  TicketRequest,
  TicketResponse,
  TrainRequest,
  TrainResponse
} from './types';

const ticketsApi = new CrudApi<TicketResponse, TicketRequest>('/api/tickets');
const passengersApi = new CrudApi<PassengerResponse, PassengerRequest>('/api/passengers');
const trainsApi = new CrudApi<TrainResponse, TrainRequest>('/api/trains');
const destinationsApi = new CrudApi<DestinationResponse, DestinationRequest>('/api/destinations');
const carriageTypesApi = new CrudApi<CarriageTypeResponse, CarriageTypeRequest>('/api/carriagetypes');

const passengerOptions = async () =>
  (await passengersApi.getAll()).map(p => ({ id: p.id, text: `${p.fullName} (${p.phone})` }));

const trainOptions = async () =>
  (await trainsApi.getAll()).map(t => ({
    id: t.id,
    text: `№${t.trainNumber} (${t.trainType}) - ${t.destinationName}`
  }));

const destinationOptions = async () =>
  (await destinationsApi.getAll()).map(d => ({ id: d.id, text: d.name }));

const carriageTypeOptions = async () =>
  (await carriageTypesApi.getAll()).map(c => ({
    id: c.id,
    text: `${c.typeName} (+${formatMoney(c.surcharge)})`
  }));

export const entities: EntityModule[] = [
  new Entity<TicketResponse, TicketRequest>(
    {
      route: 'tickets',
      title: 'Квитки',
      listTitle: 'Список зарезервованих квитків',
      description: 'Відображення інформації про рейси та розрахунок вартості',
      addLabel: 'Оформити новий квиток',
      singular: 'квитка'
    },
    ticketsApi,
    [
      { header: 'Пасажир', cell: t => `${t.passengerName} (${t.passengerPhone})` },
      { header: 'Рейс і напрямок', cell: t => `Поїзд ${t.trainNumber} (${t.trainType}) → ${t.destinationName}` },
      { header: 'Вагон', cell: t => `№${t.carriageNumber}, ${t.carriageTypeName}` },
      { header: 'Дата відправлення', cell: t => formatDate(t.departureDate) },
      { header: 'До сплати', cell: t => formatMoney(t.totalPrice), className: 'fw-bold text-success' }
    ],
    [
      { key: 'passengerId', label: 'Пасажир', kind: 'select', options: passengerOptions },
      { key: 'trainId', label: 'Потяг та напрямок', kind: 'select', options: trainOptions },
      { key: 'carriageTypeId', label: 'Тип вагона', kind: 'select', options: carriageTypeOptions },
      { key: 'carriageNumber', label: 'Номер вагона (1-30)', kind: 'number' },
      { key: 'departureDate', label: 'Дата відправлення', kind: 'date' },
      { key: 'urgencySurcharge', label: 'Надбавка за терміновість (грн)', kind: 'number' }
    ],
    () => ({
      passengerId: 0,
      trainId: 0,
      carriageNumber: 1,
      carriageTypeId: 0,
      departureDate: today(),
      urgencySurcharge: 0
    }),
    t => ({
      passengerId: t.passengerId,
      trainId: t.trainId,
      carriageNumber: t.carriageNumber,
      carriageTypeId: t.carriageTypeId,
      departureDate: t.departureDate,
      urgencySurcharge: t.urgencySurcharge
    })
  ),

  new Entity<PassengerResponse, PassengerRequest>(
    {
      route: 'passengers',
      title: 'Пасажири',
      listTitle: 'Список пасажирів',
      description: 'Контактні дані та кількість куплених квитків',
      addLabel: 'Додати пасажира',
      singular: 'пасажира'
    },
    passengersApi,
    [
      { header: 'ПІБ пасажира', cell: p => p.fullName, className: 'fw-semibold' },
      { header: 'Домашня адреса', cell: p => p.address },
      { header: 'Номер телефону', cell: p => p.phone },
      { header: 'Квитків', cell: p => String(p.ticketsCount), className: 'text-center' }
    ],
    [
      { key: 'fullName', label: 'ПІБ', kind: 'text' },
      { key: 'address', label: 'Адреса', kind: 'text' },
      { key: 'phone', label: 'Номер телефону', kind: 'text' }
    ],
    () => ({ fullName: '', address: '', phone: '' }),
    p => ({ fullName: p.fullName, address: p.address, phone: p.phone })
  ),

  new Entity<TrainResponse, TrainRequest>(
    {
      route: 'trains',
      title: 'Поїзди',
      listTitle: 'Список поїздів',
      description: 'Розклад руху та напрямки',
      addLabel: 'Додати поїзд',
      singular: 'поїзда'
    },
    trainsApi,
    [
      { header: 'Номер', cell: t => t.trainNumber, className: 'fw-semibold' },
      { header: 'Тип', cell: t => t.trainType },
      { header: 'Пункт призначення', cell: t => t.destinationName ?? '' },
      { header: 'Відправлення', cell: t => t.departureTime },
      { header: 'Прибуття', cell: t => t.arrivalTime },
      { header: 'Квитків', cell: t => String(t.ticketsCount), className: 'text-center' }
    ],
    [
      { key: 'trainNumber', label: 'Номер потяга', kind: 'text' },
      { key: 'trainType', label: 'Тип потяга', kind: 'text' },
      { key: 'destinationId', label: 'Станція призначення', kind: 'select', options: destinationOptions },
      { key: 'departureTime', label: 'Час відправлення', kind: 'text' },
      { key: 'arrivalTime', label: 'Час прибуття', kind: 'text' }
    ],
    () => ({ trainNumber: '', trainType: '', destinationId: 0, departureTime: '', arrivalTime: '' }),
    t => ({
      trainNumber: t.trainNumber,
      trainType: t.trainType,
      destinationId: t.destinationId,
      departureTime: t.departureTime,
      arrivalTime: t.arrivalTime
    })
  ),

  new Entity<DestinationResponse, DestinationRequest>(
    {
      route: 'destinations',
      title: 'Пункти призначення',
      listTitle: 'Список пунктів призначення',
      description: 'Відстань та базовий тариф',
      addLabel: 'Додати пункт призначення',
      singular: 'пункту призначення'
    },
    destinationsApi,
    [
      { header: 'Назва станції', cell: d => d.name, className: 'fw-semibold' },
      { header: 'Відстань', cell: d => `${d.distanceKm} км` },
      { header: 'Базовий тариф', cell: d => formatMoney(d.baseFare) },
      { header: 'Поїздів', cell: d => String(d.trainsCount), className: 'text-center' }
    ],
    [
      { key: 'name', label: 'Назва станції', kind: 'text' },
      { key: 'distanceKm', label: 'Відстань (км)', kind: 'number' },
      { key: 'baseFare', label: 'Базовий тариф (грн)', kind: 'number' }
    ],
    () => ({ name: '', distanceKm: 0, baseFare: 0 }),
    d => ({ name: d.name, distanceKm: d.distanceKm, baseFare: d.baseFare })
  ),

  new Entity<CarriageTypeResponse, CarriageTypeRequest>(
    {
      route: 'carriagetypes',
      title: 'Типи вагонів',
      listTitle: 'Список типів вагонів',
      description: 'Надбавки до вартості квитка',
      addLabel: 'Додати тип вагона',
      singular: 'типу вагона'
    },
    carriageTypesApi,
    [
      { header: 'Назва типу', cell: c => c.typeName, className: 'fw-semibold' },
      { header: 'Надбавка', cell: c => formatMoney(c.surcharge) },
      { header: 'Квитків', cell: c => String(c.ticketsCount), className: 'text-center' }
    ],
    [
      { key: 'typeName', label: 'Назва типу', kind: 'text' },
      { key: 'surcharge', label: 'Надбавка (грн)', kind: 'number' }
    ],
    () => ({ typeName: '', surcharge: 0 }),
    c => ({ typeName: c.typeName, surcharge: c.surcharge })
  )
];
