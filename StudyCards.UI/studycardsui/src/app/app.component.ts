import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { DeckListComponent } from './features/deck-management/components/deck-list/deck-list.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, DeckListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'studycardsui';
}
