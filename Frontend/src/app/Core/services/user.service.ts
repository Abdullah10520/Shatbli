import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
    ApiResponse,
    ProfileData,
    UpdateProfileRequest,
    ChangePasswordRequest
} from '../../shared/models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
    private readonly apiUrl = environment.apiUrl;
    private http = inject(HttpClient);

    // Profile state
    profileSig = signal<ProfileData | null>(null);
    isLoadingSig = signal(false);

    /**
     * جلب بيانات الملف الشخصي
     */
    getProfile(): Observable<ApiResponse<ProfileData>> {
        this.isLoadingSig.set(true);
        return this.http.get<ApiResponse<ProfileData>>(`${this.apiUrl}/Users/profile`)
            .pipe(
                tap(response => {
                    this.isLoadingSig.set(false);
                    if (response.success && response.data) {
                        this.profileSig.set(response.data);
                    }
                }),
                catchError(error => {
                    this.isLoadingSig.set(false);
                    throw error;
                })
            );
    }

    /**
     * تحديث بيانات الملف الشخصي
     */
    updateProfile(data: UpdateProfileRequest): Observable<ApiResponse<void>> {
        this.isLoadingSig.set(true);
        return this.http.put<ApiResponse<void>>(`${this.apiUrl}/Users/profile`, data)
            .pipe(
                tap(response => {
                    this.isLoadingSig.set(false);
                    if (response.success) {
                        // تحديث البيانات المحلية
                        const currentProfile = this.profileSig();
                        if (currentProfile) {
                            this.profileSig.set({
                                ...currentProfile,
                                fullName: data.fullName,
                                phoneNumber: data.phoneNumber
                            });
                        }
                    }
                }),
                catchError(error => {
                    this.isLoadingSig.set(false);
                    throw error;
                })
            );
    }

    /**
     * تغيير كلمة المرور
     */
    changePassword(data: ChangePasswordRequest): Observable<ApiResponse<void>> {
        return this.http.put<ApiResponse<void>>(`${this.apiUrl}/Users/change-password`, data);
    }
}
