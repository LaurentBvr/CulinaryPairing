import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecetteList } from './recette-list';

describe('RecetteList', () => {
  let component: RecetteList;
  let fixture: ComponentFixture<RecetteList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecetteList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecetteList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
