import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from './api.service';
import { IDelegate, CardHolder, InvoicesReport } from './app.models';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    cardHolders$: Observable<CardHolder[]>;
    invoicesSource$: IDelegate<InvoicesReport>;
    invoices$: Observable<InvoicesReport>;

    visible: boolean = false;
    selectedId: number;

    constructor(private _api: ApiService) {
        this.cardHolders$ = this._api.getCardHolders();
        this.invoicesSource$ = (id: number) => this.select(id);
    }

    select = (cardHolderId: number): Observable<InvoicesReport> => {
        this.visible = true;
        this.selectedId = cardHolderId;
        return this.invoices$ = this._api.getInvoicesFor(cardHolderId);
    }
}
