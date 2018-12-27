import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from './api.service';
import { UserAccount, CardHolder, InvoicesReport } from './app.models';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    user: UserAccount;
    invoices$: Observable<InvoicesReport>;

    visible: boolean = false;
    selectedId: number;

    constructor(private _api: ApiService) {
        this._api.whoAmI().subscribe(result => {
            this.user = result;
            this.invoices$ = this._api.getInvoicesFor(result.id);
        });
    }

    selectWallet = (walletId: number): void => {
        if (!walletId) { return; }
        this.visible = true;
        this.selectedId = walletId;
        this.invoices$ = this._api.getInvoicesByWalletId(this.user.cardHolderId, walletId);
    }
}
