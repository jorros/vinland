import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NameLoginComponent } from './name-login.component';

describe('NameLoginComponent', () => {
  let component: NameLoginComponent;
  let fixture: ComponentFixture<NameLoginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NameLoginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NameLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
