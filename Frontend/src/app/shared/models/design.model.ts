// أنواع التصميم
export enum DesignType {
    Ceramic = 1,
    Paint = 2
}

// نموذج التصميم (سيراميك أو لون)
export interface Design {
    id: string;
    name: string;
    imageUrl: string;
    type: DesignType;
    description?: string;
    createdAt?: string;
}

// نموذج نتيجة التوليد
export interface GeneratedDesign {
    id: string;
    originalImageUrl: string;
    generatedImageUrl: string;
    designType: DesignType;
    ceramicOrPaintId?: string;
    createdAt: string;
}

// طلب التوليد من المعرض
export interface GenerateFromGalleryRequest {
    roomImage: File;
    ceramicId: string;
    designType: DesignType;
}

// طلب التوليد بصورة مخصصة
export interface GenerateWithCustomImageRequest {
    roomImage: File;
    ceramicOrPaintImage: File;
    designType: DesignType;
}

// استجابة التوليد
export interface GenerateDesignResponse {
    success: boolean;
    generatedImageUrl: string;
    designId: string;
    message?: string;
}
