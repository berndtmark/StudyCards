import { Inject, Injectable, InjectionToken, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';

export const SIGNALR_URL = new InjectionToken<string>('SignalRHubUrl');

@Injectable({
  providedIn: 'root'
})
/* Usage Example
  private hub = inject(HubService)
  message = computed(() => this.hub.messageReceived());

  ngOnInit(): void {
    this.hub.init();
  }

  send(value: string) {
    this.hub.sendMessage("test@gmail.com", value);
  }
*/
export class HubService {
  private hubConnection!: signalR.HubConnection;
  messageReceived = signal<string | null>(null);

  constructor(@Inject(SIGNALR_URL) private hubUrl: string) {
  }

  init() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl)
      .withAutomaticReconnect()
      .build();

    this.addListeners();

    this.hubConnection.start().catch(err => console.error('SignalR Error:', err));
  }

  sendMessage(user: string, message: string) {
    if (!this.hubConnection)
      throw "SignalR connection not initialized. Call init()"

    this.hubConnection.invoke('SendMessage', user, message)
      .catch(err => console.error('Send error:', err));
  }

  private addListeners() {
    this.hubConnection.on('ReceivedMessage', (message: string) => {
      this.messageReceived.set(message);
    });
  }
}
