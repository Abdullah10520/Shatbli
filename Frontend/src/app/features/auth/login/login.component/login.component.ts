import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../../Core/auth/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);
  showPassword = signal(false);

  // Where to redirect after login (default to home page)
  private returnUrl = '/';

  loginForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  ngOnInit(): void {
    // Get return URL from query params, default to home page
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  togglePassword(): void {
    this.showPassword.update(v => !v);
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.isLoading.set(true);
      this.errorMessage.set(null);

      const credentials = this.loginForm.getRawValue();

      this.authService.login(credentials).subscribe({
        next: (response) => {
          if (response.success) {
            // Navigate to returnUrl (studio if clicked from studio, home if clicked from login button)
            this.router.navigateByUrl(this.returnUrl);
          } else {
            this.isLoading.set(false);
            this.errorMessage.set(response.message || 'خطأ في البريد الإلكتروني أو كلمة المرور');
          }
        },
        error: (err) => {
          this.isLoading.set(false);
          this.errorMessage.set(err.error?.message || 'خطأ في البريد الإلكتروني أو كلمة المرور');
        },
      });
    }
  }
}

