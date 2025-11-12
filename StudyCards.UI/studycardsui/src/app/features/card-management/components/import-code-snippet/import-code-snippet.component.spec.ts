import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { ImportCodeSnippetComponent } from './import-code-snippet.component';

describe('ImportCodeSnippetComponent', () => {
  let component: ImportCodeSnippetComponent;
  let fixture: ComponentFixture<ImportCodeSnippetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImportCodeSnippetComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ImportCodeSnippetComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
