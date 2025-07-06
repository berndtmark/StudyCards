import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportCodeSnippetComponent } from './import-code-snippet.component';

describe('ImportCodeSnippetComponent', () => {
  let component: ImportCodeSnippetComponent;
  let fixture: ComponentFixture<ImportCodeSnippetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImportCodeSnippetComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ImportCodeSnippetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
