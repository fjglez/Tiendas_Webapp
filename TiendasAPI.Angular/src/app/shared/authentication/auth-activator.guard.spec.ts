import { TestBed } from '@angular/core/testing';

import { AuthActivatorGuard } from './auth-activator.guard';

describe('AuthActivatorGuard', () => {
  let guard: AuthActivatorGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(AuthActivatorGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
