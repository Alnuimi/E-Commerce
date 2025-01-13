import {nanoid} from 'nanoid';

export type CartType = {
    id: string;
    items: CartItem[];
    DeliveryMethodId?: number;
    clientSecret?: string;
    PaymentIntentId?: string;
}

export type CartItem = {
    productId: number;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    brand: string;
    type: string;
}

export class Cart implements CartType {
    id = nanoid();
    items: CartItem[] = [];
    DeliveryMethodId?: number;
    clientSecret?: string;
    PaymentIntentId?: string;
}