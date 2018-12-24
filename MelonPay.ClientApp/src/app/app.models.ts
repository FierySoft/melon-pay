import { Observable } from 'rxjs';

export interface IDelegate<T> {
    (id: number): Observable<T>;
}

export class Currency {
    id: number;
    code: string;
    name: string;
}

enum Gender {
    Male,
    Female
}

export class CardHolder {
    id: number;
    firstName: string;
    middleName: string;
    lastName: string;
    fullName: string;
    gender: Gender;
    isDeleted: boolean;
    wallets: Wallet[];
}

export class Wallet {
    id: number;
    currencyId: number;
    currency: Currency;
    cardHolderId: number;
    cardHolder: CardHolder;
    balance: number;
    isDeleted: boolean;
}

export class InvoiceStatus {
    id: number;
    code: string;
    name: string;
}

export class Invoice {
    id: number;
    amount: number;
    comment: string;
    createdAt: Date;
    fromWalletId?: number;
    fromWallet: Wallet;
    toWalletId?: number;
    toWallet: Wallet;
    statusId: number;
    status: InvoiceStatus;
    reason: string;
}

export interface InvoicesReport {
    cardHolderId: number;
    cardHolder: CardHolder;
    sended: Invoice[];
    received: Invoice[];
}

export interface InvoiceCreate {
    fromWalletId: number;
    toWalletId: number;
    amount: number;
    comment: string;
}
