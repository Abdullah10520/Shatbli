import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';

import { SubscriptionService } from '../../Core/services/subscription.service';
import { AuthService } from '../../Core/auth/auth.service';
import { ThemeService } from '../../Core/services/theme.service';
import { LanguageService } from '../../Core/services/language.service';
import { SubscriptionPlan } from '../../shared/models/subscription.model';

@Component({
    selector: 'app-subscription',
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        TranslatePipe
    ],
    templateUrl: './subscription.component.html',
    styleUrl: './subscription.component.css'
})
export class SubscriptionComponent implements OnInit {
    private subscriptionService = inject(SubscriptionService);
    private authService = inject(AuthService);
    readonly themeService = inject(ThemeService);
    readonly languageService = inject(LanguageService);
    private translate = inject(TranslateService);
    private router = inject(Router);

    // State from service
    plans = this.subscriptionService.plansSig;
    currentSubscription = this.subscriptionService.currentSubscriptionSig;
    isLoading = this.subscriptionService.isLoadingSig;
    isProcessing = this.subscriptionService.isProcessingSig;

    // Sorted plans: Free -> Basic -> Premium
    sortedPlans = computed(() => {
        const planOrder = { 'Free': 0, 'Basic': 1, 'Premium': 2 };
        return [...this.plans()].sort((a, b) =>
            (planOrder[a.type] ?? 99) - (planOrder[b.type] ?? 99)
        );
    });

    // Local state
    successMessage = signal<string | null>(null);
    errorMessage = signal<string | null>(null);
    processingPlanId = signal<string | null>(null);

    ngOnInit(): void {
        this.loadData();
    }

    loadData(): void {
        this.subscriptionService.getPlans().subscribe();
        this.subscriptionService.getMySubscription().subscribe();
    }

    refreshSubscription(): void {
        this.subscriptionService.getMySubscription().subscribe();
    }

    subscribeToPlan(plan: SubscriptionPlan): void {
        this.clearMessages();
        this.processingPlanId.set(plan.id);

        this.subscriptionService.subscribe(plan.id).subscribe({
            next: (response) => {
                this.processingPlanId.set(null);
                if (response.success) {
                    this.successMessage.set(response.message);
                    this.refreshSubscription();
                    this.clearMessageAfterDelay();
                } else {
                    this.errorMessage.set(response.message);
                }
            },
            error: (err) => {
                this.processingPlanId.set(null);
                this.errorMessage.set(err.error?.message || this.translate.instant('subscription.error_subscribing'));
            }
        });
    }

    cancelSubscription(): void {
        this.clearMessages();

        this.subscriptionService.cancelSubscription().subscribe({
            next: (response) => {
                if (response.success) {
                    this.successMessage.set(response.message);
                    this.refreshSubscription();
                    this.clearMessageAfterDelay();
                } else {
                    this.errorMessage.set(response.message);
                }
            },
            error: (err) => {
                this.errorMessage.set(err.error?.message || this.translate.instant('subscription.error_cancelling'));
            }
        });
    }

    isCurrentPlan(plan: SubscriptionPlan): boolean {
        return this.currentSubscription()?.planType === plan.type;
    }

    isPlanProcessing(planId: string): boolean {
        return this.processingPlanId() === planId;
    }

    getPlanIcon(type: string): string {
        switch (type) {
            case 'Free': return '🎁';
            case 'Basic': return '⚡';
            case 'Premium': return '👑';
            default: return '📦';
        }
    }

    getFeatures(plan: SubscriptionPlan): { text: string; included: boolean }[] {
        const isArabic = this.languageService.currentLang() === 'ar';

        const features = [
            {
                text: plan.maxImagesPerMonth === -1
                    ? (isArabic ? 'صور غير محدودة شهرياً' : 'Unlimited images/month')
                    : (isArabic ? `${plan.maxImagesPerMonth} صورة شهرياً` : `${plan.maxImagesPerMonth} images/month`),
                included: true
            },
            {
                text: plan.maxImagesPerDay === -1
                    ? (isArabic ? 'صور غير محدودة يومياً' : 'Unlimited images/day')
                    : (isArabic ? `${plan.maxImagesPerDay} صور يومياً` : `${plan.maxImagesPerDay} images/day`),
                included: true
            },
            {
                text: isArabic ? 'بدون علامة مائية' : 'No watermark',
                included: !plan.hasWatermark
            },
            {
                text: isArabic ? 'أولوية في التوليد' : 'Priority generation',
                included: plan.hasPriorityGeneration
            }
        ];

        return features;
    }

    private clearMessages(): void {
        this.successMessage.set(null);
        this.errorMessage.set(null);
    }

    private clearMessageAfterDelay(): void {
        setTimeout(() => {
            this.successMessage.set(null);
        }, 5000);
    }

    goToStudio(): void {
        this.router.navigate(['/studio']);
    }

    goToProfile(): void {
        this.router.navigate(['/profile']);
    }

    logout(): void {
        this.authService.logout();
        this.router.navigate(['/auth/login']);
    }
}
