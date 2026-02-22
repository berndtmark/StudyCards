import { Injectable, signal } from '@angular/core';

export interface NavMenu {
  name: string;
  icon: string;
  route: string;
}

@Injectable({
  providedIn: 'root',
})
export class NavMenuService {
  private _navMenu = signal<NavMenu[]>([
    { name: 'Home', icon: 'home', route: '/decks' },
    { name: 'Study Calandar', icon: 'show_chart', route: '/statistics' },
  ]);

  public navMenu = this._navMenu.asReadonly();
}
