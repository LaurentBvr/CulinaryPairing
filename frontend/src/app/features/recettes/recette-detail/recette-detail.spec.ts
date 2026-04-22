import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecetteDetail } from './recette-detail';

describe('RecetteDetail', () => {
  let component: RecetteDetail;
  let fixture: ComponentFixture<RecetteDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecetteDetail]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecetteDetail);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
