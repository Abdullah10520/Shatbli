import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
    Design,
    DesignType,
    GeneratedDesign,
    GenerateDesignResponse
} from '../../shared/models/design.model';

@Injectable({ providedIn: 'root' })
export class DesignService {
    private readonly apiUrl = environment.apiUrl;
    private http = inject(HttpClient);

    // حالة المعرض
    designsSig = signal<Design[]>([]);
    isLoadingSig = signal(false);

    // حالة التوليد
    isGeneratingSig = signal(false);
    generatedResultSig = signal<GeneratedDesign | null>(null);

    /**
     * جلب كل التصميمات (سيراميك وألوان)
     */
    getAllDesigns(): Observable<Design[]> {
        this.isLoadingSig.set(true);
        return this.http.get<Design[]>(`${this.apiUrl}/Design/GetAllDesigns`)
            .pipe(
                tap(designs => {
                    this.designsSig.set(designs);
                    this.isLoadingSig.set(false);
                })
            );
    }

    /**
     * جلب تفاصيل تصميم معين
     */
    getDesignById(designId: string): Observable<Design> {
        return this.http.get<Design>(`${this.apiUrl}/Design/GetDesignById`, {
            params: { designId }
        });
    }

    /**
     * توليد تصميم من المعرض
     * @param roomImage صورة الغرفة
     * @param ceramicId معرف السيراميك/اللون من المعرض
     * @param designType نوع التصميم (1=سيراميك، 2=لون)
     */
    generateDesign(
        roomImage: File,
        ceramicId: string,
        designType: DesignType
    ): Observable<GenerateDesignResponse> {
        this.isGeneratingSig.set(true);

        const formData = new FormData();
        formData.append('roomImageFile', roomImage);

        return this.http.post<GenerateDesignResponse>(
            `${this.apiUrl}/Design/GenerateDesign`,
            formData,
            { params: { ceramicId, designType: designType.toString() } }
        ).pipe(
            tap(response => {
                console.log('📥 GenerateDesign Response:', response);
                console.log('📥 Response Type:', typeof response);
                this.isGeneratingSig.set(false);
                if (response.success) {
                    this.generatedResultSig.set({
                        id: response.designId,
                        originalImageUrl: URL.createObjectURL(roomImage),
                        generatedImageUrl: response.generatedImageUrl,
                        designType,
                        ceramicOrPaintId: ceramicId,
                        createdAt: new Date().toISOString()
                    });
                }
            })
        );
    }

    /**
     * توليد تصميم بصورة مخصصة
     * @param roomImage صورة الغرفة
     * @param ceramicOrPaintImage صورة السيراميك/اللون المخصصة
     * @param designType نوع التصميم (1=سيراميك، 2=لون)
     */
    generateDesignWithUserImage(
        roomImage: File,
        ceramicOrPaintImage: File,
        designType: DesignType
    ): Observable<GenerateDesignResponse> {
        this.isGeneratingSig.set(true);

        const formData = new FormData();
        formData.append('roomImageFile', roomImage);
        formData.append('ceramicOrPaintImageFile', ceramicOrPaintImage);

        return this.http.post<GenerateDesignResponse>(
            `${this.apiUrl}/Design/GenerateDesignWithUserCeramicImage`,
            formData,
            { params: { designType: '1' } }
        ).pipe(
            tap(response => {
                console.log('📥 GenerateDesignWithUserImage Response:', response);
                console.log('📥 Response Type:', typeof response);
                this.isGeneratingSig.set(false);
                if (response.success) {
                    this.generatedResultSig.set({
                        id: response.designId,
                        originalImageUrl: URL.createObjectURL(roomImage),
                        generatedImageUrl: response.generatedImageUrl,
                        designType,
                        createdAt: new Date().toISOString()
                    });
                }
            })
        );
    }

    /**
     * مسح النتيجة الحالية
     */
    clearResult(): void {
        this.generatedResultSig.set(null);
    }
}
