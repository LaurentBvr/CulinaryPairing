import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideFrigo } from './vide-frigo';

describe('VideFrigo', () => {
  let component: VideFrigo;
  let fixture: ComponentFixture<VideFrigo>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VideFrigo]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VideFrigo);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
