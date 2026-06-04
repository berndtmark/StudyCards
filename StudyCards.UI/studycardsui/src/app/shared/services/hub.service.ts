import { inject, Service, InjectionToken, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';

export const SIGNALR_URL = new InjectionToken<string>('SignalRHubUrl');

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
@Service()
export class HubService {
  private hubConnection!: signalR.HubConnection;
  messageReceived = signal<string | null>(null);

  private hubUrl = inject(SIGNALR_URL);

  init() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl)
      .withAutomaticReconnect()
      .build();

    this.addListeners();

    this.hubConnection.start().catch(err => console.error('SignalR Error:', err));
  }

  destroy() {
    if (this.hubConnection)
      this.hubConnection.stop().catch(err => console.error('SignalR Error:', err));
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
