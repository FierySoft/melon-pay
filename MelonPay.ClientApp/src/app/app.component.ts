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
    user$: Observable<UserAccount>;
    users$: Observable<UserAccount[]>;
    invoices$: Observable<InvoicesReport>;

    walletsVisible: boolean = false;
    selectedWalletId: number;

    constructor(private _api: ApiService) {
        this.changeUser(1); // set default signed-in user
        this.users$ = this._api.getAllUsers();
    }

    changeUser = (userId: number): void => {
        if (!userId) { return; }
        this.walletsVisible = false;
        this.selectedWalletId = null;
        this.user$ = this._api.whoAmI(userId);
        this.invoices$ = this._api.getInvoicesFor(userId);
    }

    selectWallet = (walletId: number): void => {
        if (!walletId) { return; }
        this.walletsVisible = true;
        this.selectedWalletId = walletId;
        this.user$.subscribe(user =>
            this.invoices$ = this._api.getInvoicesByWalletId(user.cardHolderId, walletId)
        );
    }
}
