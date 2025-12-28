import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';

import { UserService } from '../../Core/services/user.service';
import { AuthService } from '../../Core/auth/auth.service';
import { ThemeService } from '../../Core/services/theme.service';
import { LanguageService } from '../../Core/services/language.service';
import { ProfileData } from '../../shared/models/user.model';

@Component({
    selector: 'app-profile',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule,
        TranslatePipe
    ],
    templateUrl: './profile.component.html',
    styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
    private fb = inject(FormBuilder);
    private userService = inject(UserService);
    private authService = inject(AuthService);
    readonly themeService = inject(ThemeService);
    readonly languageService = inject(LanguageService);
    private translate = inject(TranslateService);
    private router = inject(Router);

    // State
    profile = this.userService.profileSig;
    isLoading = this.userService.isLoadingSig;
    currentUser = this.authService.currentUserSig;

    // Edit profile state
    isEditingProfile = signal(false);
    isSavingProfile = signal(false);
    profileErrors = signal<Record<string, string[]>>({});
    profileSuccessMessage = signal<string | null>(null);

    // Change password state
    isChangingPassword = signal(false);
    isSavingPassword = signal(false);
    passwordErrors = signal<Record<string, string[]>>({});
    passwordSuccessMessage = signal<string | null>(null);
    showCurrentPassword = signal(false);
    showNewPassword = signal(false);
    showConfirmPassword = signal(false);

    // Forms
    profileForm = this.fb.nonNullable.group({
        fullName: ['', [Validators.required, Validators.minLength(3)]],
        phoneNumber: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]]
    });

    passwordForm = this.fb.nonNullable.group({
        currentPassword: ['', [Validators.required]],
        newPassword: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', [Validators.required]]
    });

    ngOnInit(): void {
        this.loadProfile();
    }

    loadProfile(): void {
        this.userService.getProfile().subscribe({
            next: (response) => {
                if (response.success && response.data) {
                    this.profileForm.patchValue({
                        fullName: response.data.fullName,
                        phoneNumber: response.data.phoneNumber || ''
                    });
                }
            },
            error: (err) => {
                console.error('Failed to load profile', err);
            }
        });
    }

    // Edit profile methods
    startEditing(): void {
        this.isEditingProfile.set(true);
        this.profileErrors.set({});
        this.profileSuccessMessage.set(null);
    }

    cancelEditing(): void {
        this.isEditingProfile.set(false);
        this.profileErrors.set({});
        // Reset form to original values
        const profile = this.profile();
        if (profile) {
            this.profileForm.patchValue({
                fullName: profile.fullName,
                phoneNumber: profile.phoneNumber || ''
            });
        }
    }

    saveProfile(): void {
        if (this.profileForm.invalid) return;

        this.isSavingProfile.set(true);
        this.profileErrors.set({});
        this.profileSuccessMessage.set(null);

        const data = this.profileForm.getRawValue();

        this.userService.updateProfile(data).subscribe({
            next: (response) => {
                this.isSavingProfile.set(false);
                if (response.success) {
                    this.isEditingProfile.set(false);
                    this.profileSuccessMessage.set(this.translate.instant('profile.profile_updated'));

                    // Update currentUser in auth service
                    const currentUser = this.currentUser();
                    if (currentUser) {
                        this.authService.currentUserSig.set({
                            ...currentUser,
                            fullName: data.fullName,
                            phoneNumber: data.phoneNumber
                        });
                        // Update localStorage too
                        localStorage.setItem('user', JSON.stringify({
                            ...currentUser,
                            fullName: data.fullName,
                            phoneNumber: data.phoneNumber
                        }));
                    }

                    // Clear success message after 3 seconds
                    setTimeout(() => this.profileSuccessMessage.set(null), 3000);
                } else {
                    this.profileErrors.set(response.errors || {});
                }
            },
            error: (err) => {
                this.isSavingProfile.set(false);
                if (err.error?.errors) {
                    this.profileErrors.set(err.error.errors);
                } else {
                    this.profileErrors.set({ general: [err.error?.message || 'حدث خطأ غير متوقع'] });
                }
            }
        });
    }

    // Change password methods
    startChangingPassword(): void {
        this.isChangingPassword.set(true);
        this.passwordErrors.set({});
        this.passwordSuccessMessage.set(null);
        this.passwordForm.reset();
    }

    cancelChangingPassword(): void {
        this.isChangingPassword.set(false);
        this.passwordErrors.set({});
        this.passwordForm.reset();
    }

    savePassword(): void {
        if (this.passwordForm.invalid) return;

        const { currentPassword, newPassword, confirmPassword } = this.passwordForm.getRawValue();

        // Check password confirmation
        if (newPassword !== confirmPassword) {
            this.passwordErrors.set({ confirmPassword: [this.translate.instant('errors.password_mismatch')] });
            return;
        }

        this.isSavingPassword.set(true);
        this.passwordErrors.set({});
        this.passwordSuccessMessage.set(null);

        this.userService.changePassword({ currentPassword, newPassword }).subscribe({
            next: (response) => {
                this.isSavingPassword.set(false);
                if (response.success) {
                    this.isChangingPassword.set(false);
                    this.passwordSuccessMessage.set(this.translate.instant('profile.password_changed'));
                    this.passwordForm.reset();

                    // Clear success message after 3 seconds
                    setTimeout(() => this.passwordSuccessMessage.set(null), 3000);
                } else {
                    this.passwordErrors.set(response.errors || {});
                }
            },
            error: (err) => {
                this.isSavingPassword.set(false);
                if (err.error?.errors) {
                    this.passwordErrors.set(err.error.errors);
                } else {
                    this.passwordErrors.set({ general: [err.error?.message || 'حدث خطأ غير متوقع'] });
                }
            }
        });
    }

    toggleCurrentPassword(): void {
        this.showCurrentPassword.update(v => !v);
    }

    toggleNewPassword(): void {
        this.showNewPassword.update(v => !v);
    }

    toggleConfirmPassword(): void {
        this.showConfirmPassword.update(v => !v);
    }

    // Helper to get field errors from API response
    getFieldErrors(fieldName: string, errorsObj: Record<string, string[]>): string[] {
        return errorsObj[fieldName] || [];
    }

    logout(): void {
        this.authService.logout();
        this.router.navigate(['/auth/login']);
    }

    goToStudio(): void {
        this.router.navigate(['/studio']);
    }
}
