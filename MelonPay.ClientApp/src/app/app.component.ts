import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from './api.service';
import { CardHolder, InvoicesReport } from './app.models';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    cardHolders$: Observable<CardHolder[]>;
    invoices$: Observable<InvoicesReport>;

    visible: boolean = false;
    selectedId: number;

    constructor(private _api: ApiService) {
        this.cardHolders$ = this._api.getCardHolders();
    }

    select = (cardHolderId: number): void => {
        this.visible = true;
        this.selectedId = cardHolderId;
        this.invoices$ = this._api.getInvoicesFor(cardHolderId);
    }
}
