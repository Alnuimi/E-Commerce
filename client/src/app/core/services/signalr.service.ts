import { Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { Order } from '../../shared/models/order';
@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  hubUrl = environment.hubUrl;
  hubConnections?: HubConnection;
  orderSignal = signal<Order | null>(null);
  createHubConnection() {
    this.hubConnections = new HubConnectionBuilder()
    .withUrl(this.hubUrl , {
      withCredentials: true
    })
    .withAutomaticReconnect()
    .build();

    this.hubConnections.start()
      .catch(error => console.log(error));

    this.hubConnections.on('OrderCompleteNotification', (order: Order) => {
      this.orderSignal.set(order)
    });
  }

  stopHubConnection() {
    if(this.hubConnections?.state === HubConnectionState.Connected) {
      this.hubConnections.stop().catch(error => console.log(error));
    }
  }
}
