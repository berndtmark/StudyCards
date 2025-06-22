import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AddUpdateCardBaseComponent } from '../add-update-card-base.component';
import { BackNavComponent } from '../../../../../shared/components/back-nav/back-nav.component';
@Component({
    selector: 'app-add-card-form',
    imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, BackNavComponent],
    templateUrl: '../add-update-card-form.component.html',
    styleUrl: '../add-update-card-form.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddCardFormComponent extends AddUpdateCardBaseComponent implements OnInit {
    saveButtonName = "Add Card";
    includeRemove = false;
    title = "Add Card";

    ngOnInit(): void {
        this.initForm();
    }

    onSubmit(): void {
        if (this.cardForm.valid) {
            const form = this.cardForm.value;
            this.store.addCard({ 
                cardFront: form.front, 
                cardBack: form.back,
            });
            
            this.cardForm.reset();
            this.goBackToCardList();
        }
    }
}
