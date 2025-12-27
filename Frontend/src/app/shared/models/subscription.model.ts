// Subscription Plan interface
export interface SubscriptionPlan {
    id: string;
    name: string;
    type: 'Free' | 'Basic' | 'Premium';
    price: number;
    maxImagesPerMonth: number;
    maxImagesPerDay: number;
    hasWatermark: boolean;
    hasPriorityGeneration: boolean;
    description: string;
}

// User's current subscription
export interface UserSubscription {
    id: string;
    planName: string;
    planType: 'Free' | 'Basic' | 'Premium';
    price: number;
    startDate: string;
    isActive: boolean;
    imagesGeneratedToday: number;
    imagesGeneratedThisMonth: number;
    maxImagesPerDay: number;
    maxImagesPerMonth: number;
}

// API Response types
export interface GetPlansResponse {
    success: boolean;
    message: string;
    data: SubscriptionPlan[];
    errors: Record<string, string[]>;
    timestamp: string;
}

export interface GetMySubscriptionResponse {
    success: boolean;
    message: string;
    data: UserSubscription;
    errors: Record<string, string[]>;
    timestamp: string;
}

export interface SubscribeRequest {
    planId: string;
}

export interface SubscribeResponse {
    success: boolean;
    message: string;
    errors: Record<string, string[]>;
    timestamp: string;
}

export interface CancelSubscriptionResponse {
    success: boolean;
    message: string;
    errors: Record<string, string[]>;
    timestamp: string;
}
