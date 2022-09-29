import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassCartComponent } from './class-cart.component';

describe('ClassCartComponent', () => {
  let component: ClassCartComponent;
  let fixture: ComponentFixture<ClassCartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClassCartComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassCartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
