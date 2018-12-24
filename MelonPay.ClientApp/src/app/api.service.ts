import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { CardHolder, InvoicesReport, Invoice, InvoiceStatus, InvoiceCreate } from './app.models';

@Injectable({providedIn: 'root'})
export class ApiService {
    private _url: string;

    constructor(
        @Inject('BASE_URL') baseUrl: string,
        private _http: HttpClient
    ) {
        this._url = `${baseUrl}/api`;
    }

    public getCardHolders = (): Observable<CardHolder[]> =>
        this._http.get<CardHolder[]>(`${this._url}/cardholders`)

    public getCardHolder = (id: number): Observable<CardHolder> =>
        this._http.get<CardHolder>(`${this._url}/cardholders/${id}`)

    public getInvoicesFor = (cardHolderId: number): Observable<InvoicesReport> =>
        this._http.get<InvoicesReport>(`${this._url}/invoices/${cardHolderId}`)

    public createInvoice = (invoice: InvoiceCreate): Observable<Invoice> =>
        this._http.post<Invoice>(`${this._url}/invoices/create`, invoice)

    public payInvoice = (id: number): Observable<InvoiceStatus> =>
        this._http.post<InvoiceStatus>(`${this._url}/invoices/pay/${id}`, { })

    public declineInvoice = (id: number): Observable<InvoiceStatus> =>
        this._http.post<InvoiceStatus>(`${this._url}/invoices/decline/${id}`, { })
}
