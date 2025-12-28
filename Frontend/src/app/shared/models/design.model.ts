// أنواع التصميم
export enum DesignType {
    Ceramic = 1,
    Paint = 2,
    CeramicAndPaint = 3
}

// استجابة جلب السيراميك
export interface CeramicsResponse {
    success: boolean;
    message: string;
    data: {
        ceramicList: CeramicProduct[];
    };
    errors: Record<string, string[]>;
    timestamp: string;
}

// استجابة جلب الألوان (غير مستخدمة حالياً - نستخدم Color Picker)
export interface PaintsResponse {
    success: boolean;
    message: string;
    data: {
        paintList: CeramicProduct[];
    };
    errors: Record<string, string[]>;
    timestamp: string;
}

// نموذج السيراميك/اللون من الـ API
export interface CeramicProduct {
    productId: string;
    productName: string;
    productImageUrl: string;
}

// نموذج السيراميك للاستخدام الداخلي
export interface Ceramic {
    id: string;
    name: string;
    imageUrl: string;
    isActive?: boolean;
}

// نموذج نتيجة التوليد
export interface GeneratedDesign {
    designId: string;
    originalImageUrl: string;
    generatedImageUrl: string;
    isSaved: boolean;
    createdAt: string;
}

// استجابة التوليد (الفورمات الجديد)
export interface GenerateDesignResponse {
    success: boolean;
    message: string;
    data: {
        generatedImagePath: string;
        designId: string;
    };
    errors: Record<string, string[]>;
    timestamp: string;
}

// استجابة حفظ التصميم
export interface SaveDesignResponse {
    success: boolean;
    message: string;
    data: {
        designId: string;
        generatedImageUrl: string;
    };
    errors: Record<string, string[]>;
    timestamp: string;
}

// تصميم محفوظ في المفضلة
export interface SavedDesign {
    designId: string;
    generatedImageUrl: string;
    completedAt: string;
}

// استجابة جلب التصاميم المحفوظة
export interface GetAllDesignsResponse {
    success: boolean;
    message: string;
    data: {
        designsList: SavedDesign[];
    };
    errors: Record<string, string[]>;
    timestamp: string;
}

// استجابة حذف التصميم
export interface DeleteDesignResponse {
    success: boolean;
    message: string;
    data: {
        success: boolean;
    };
    errors: Record<string, string[]>;
    timestamp: string;
}
