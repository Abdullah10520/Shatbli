import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
    SubscriptionPlan,
    UserSubscription,
    GetPlansResponse,
    GetMySubscriptionResponse,
    SubscribeResponse,
    CancelSubscriptionResponse
} from '../../shared/models/subscription.model';

@Injectable({ providedIn: 'root' })
export class SubscriptionService {
    private readonly apiUrl = environment.apiUrl;
    private http = inject(HttpClient);

    // Signals for reactive state
    plansSig = signal<SubscriptionPlan[]>([]);
    currentSubscriptionSig = signal<UserSubscription | null>(null);
    isLoadingSig = signal(false);
    isProcessingSig = signal(false);

    // Cache flag to avoid reloading plans every time
    private plansLoaded = false;

    /**
     * Get all available subscription plans (cached after first load)
     */
    getPlans(forceRefresh = false): Observable<GetPlansResponse> {
        // If plans are already loaded and no force refresh, return cached plans
        if (this.plansLoaded && this.plansSig().length > 0 && !forceRefresh) {
            return of({
                success: true,
                message: 'Plans loaded from cache',
                data: this.plansSig(),
                errors: {},
                timestamp: new Date().toISOString()
            });
        }

        this.isLoadingSig.set(true);
        return this.http.get<GetPlansResponse>(`${this.apiUrl}/Subscription/GetPlans`)
            .pipe(
                tap(response => {
                    if (response.success && response.data) {
                        this.plansSig.set(response.data);
                        this.plansLoaded = true;
                    }
                    this.isLoadingSig.set(false);
                }),
                catchError(error => {
                    this.isLoadingSig.set(false);
                    console.error('Failed to fetch plans:', error);
                    return of({
                        success: false,
                        message: 'Failed to fetch plans',
                        data: [],
                        errors: {},
                        timestamp: new Date().toISOString()
                    });
                })
            );
    }

    /**
     * Get current user's subscription
     */
    getMySubscription(): Observable<GetMySubscriptionResponse> {
        this.isLoadingSig.set(true);
        return this.http.get<GetMySubscriptionResponse>(`${this.apiUrl}/Subscription/GetMySubscription`)
            .pipe(
                tap(response => {
                    if (response.success && response.data) {
                        this.currentSubscriptionSig.set(response.data);
                    }
                    this.isLoadingSig.set(false);
                }),
                catchError(error => {
                    this.isLoadingSig.set(false);
                    console.error('Failed to fetch subscription:', error);
                    return of({
                        success: false,
                        message: 'Failed to fetch subscription',
                        data: null as any,
                        errors: {},
                        timestamp: new Date().toISOString()
                    });
                })
            );
    }

    /**
     * Subscribe to a plan
     */
    subscribe(planId: string): Observable<SubscribeResponse> {
        this.isProcessingSig.set(true);
        return this.http.post<SubscribeResponse>(`${this.apiUrl}/Subscription/Subscribe`, { planId })
            .pipe(
                tap(response => {
                    this.isProcessingSig.set(false);
                }),
                catchError(error => {
                    this.isProcessingSig.set(false);
                    console.error('Failed to subscribe:', error);
                    return of({
                        success: false,
                        message: error.error?.message || 'Failed to subscribe',
                        errors: error.error?.errors || {},
                        timestamp: new Date().toISOString()
                    });
                })
            );
    }

    /**
     * Cancel current subscription
     */
    cancelSubscription(): Observable<CancelSubscriptionResponse> {
        this.isProcessingSig.set(true);
        return this.http.post<CancelSubscriptionResponse>(`${this.apiUrl}/Subscription/CancelSubscription`, {})
            .pipe(
                tap(response => {
                    this.isProcessingSig.set(false);
                }),
                catchError(error => {
                    this.isProcessingSig.set(false);
                    console.error('Failed to cancel subscription:', error);
                    return of({
                        success: false,
                        message: error.error?.message || 'Failed to cancel subscription',
                        errors: error.error?.errors || {},
                        timestamp: new Date().toISOString()
                    });
                })
            );
    }
}
