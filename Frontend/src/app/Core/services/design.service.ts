import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
    Ceramic,
    CeramicsResponse,
    PaintsResponse,
    DesignType,
    GeneratedDesign,
    GenerateDesignResponse
} from '../../shared/models/design.model';

@Injectable({ providedIn: 'root' })
export class DesignService {
    private readonly apiUrl = environment.apiUrl;
    private http = inject(HttpClient);

    // حالة السيراميك
    ceramicsSig = signal<Ceramic[]>([]);
    paintsSig = signal<Ceramic[]>([]);
    isLoadingSig = signal(false);

    // حالة التوليد
    isGeneratingSig = signal(false);
    generatedResultSig = signal<GeneratedDesign | null>(null);

    /**
     * جلب كل السيراميك
     */
    getAllCeramics(): Observable<CeramicsResponse> {
        this.isLoadingSig.set(true);
        return this.http.get<CeramicsResponse>(`${this.apiUrl}/Product/GetAllCeramics`)
            .pipe(
                tap(response => {
                    // فلترة العناصر النشطة فقط
                    const activeCeramics = response.ceramicList.filter(c => c.isActive);
                    this.ceramicsSig.set(activeCeramics);
                    this.isLoadingSig.set(false);
                }),
                catchError(error => {
                    this.isLoadingSig.set(false);
                    return of({ ceramicList: [] });
                })
            );
    }

    /**
     * جلب كل الألوان
     * (افتراضي: نفس endpoint السيراميك مع تغيير الاسم)
     */
    getAllPaints(): Observable<PaintsResponse> {
        this.isLoadingSig.set(true);
        return this.http.get<PaintsResponse>(`${this.apiUrl}/Product/GetAllPaints`)
            .pipe(
                tap(response => {
                    // فلترة العناصر النشطة فقط
                    const activePaints = response.paintList.filter(p => p.isActive);
                    this.paintsSig.set(activePaints);
                    this.isLoadingSig.set(false);
                }),
                catchError(error => {
                    this.isLoadingSig.set(false);
                    return of({ paintList: [] });
                })
            );
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
        formData.append('imageFile', roomImage);

        return this.http.post<GenerateDesignResponse>(
            `${this.apiUrl}/Design/GenerateDesign`,
            formData,
            { params: { designType: designType.toString(), ceramicId } }
        ).pipe(
            tap(response => {
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
            }),
            catchError(error => {
                this.isGeneratingSig.set(false);
                throw error;
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
            { params: { designType: designType.toString() } }
        ).pipe(
            tap(response => {
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
            }),
            catchError(error => {
                this.isGeneratingSig.set(false);
                throw error;
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
