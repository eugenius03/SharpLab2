export interface Identifiable {
  id: number;
}

export interface TicketResponse extends Identifiable {
  passengerId: number;
  passengerName: string | null;
  passengerPhone: string | null;
  trainId: number;
  trainNumber: string | null;
  trainType: string | null;
  destinationName: string | null;
  carriageNumber: number;
  carriageTypeId: number;
  carriageTypeName: string | null;
  departureDate: string;
  urgencySurcharge: number;
  totalPrice: number;
}

export interface TicketRequest {
  passengerId: number;
  trainId: number;
  carriageNumber: number;
  carriageTypeId: number;
  departureDate: string;
  urgencySurcharge: number;
}

export interface PassengerResponse extends Identifiable {
  fullName: string;
  address: string;
  phone: string;
  ticketsCount: number;
}

export interface PassengerRequest {
  fullName: string;
  address: string;
  phone: string;
}

export interface TrainResponse extends Identifiable {
  trainNumber: string;
  trainType: string;
  destinationId: number;
  destinationName: string | null;
  departureTime: string;
  arrivalTime: string;
  ticketsCount: number;
}

export interface TrainRequest {
  trainNumber: string;
  trainType: string;
  destinationId: number;
  departureTime: string;
  arrivalTime: string;
}

export interface DestinationResponse extends Identifiable {
  name: string;
  distanceKm: number;
  baseFare: number;
  trainsCount: number;
}

export interface DestinationRequest {
  name: string;
  distanceKm: number;
  baseFare: number;
}

export interface CarriageTypeResponse extends Identifiable {
  typeName: string;
  surcharge: number;
  ticketsCount: number;
}

export interface CarriageTypeRequest {
  typeName: string;
  surcharge: number;
}
