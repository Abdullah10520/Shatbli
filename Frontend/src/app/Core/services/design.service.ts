import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
    Ceramic,
    CeramicsResponse,
    GeneratedDesign,
    GenerateDesignResponse,
    SaveDesignResponse,
    GetAllDesignsResponse,
    DeleteDesignResponse,
    SavedDesign
} from '../../shared/models/design.model';

@Injectable({ providedIn: 'root' })
export class DesignService {
    private readonly apiUrl = environment.apiUrl;
    private http = inject(HttpClient);

    // حالة السيراميك
    ceramicsSig = signal<Ceramic[]>([]);
    isLoadingSig = signal(false);

    // حالة التوليد
    isGeneratingSig = signal(false);
    generatedResultSig = signal<GeneratedDesign | null>(null);

    // التصاميم المحفوظة
    savedDesignsSig = signal<SavedDesign[]>([]);
    isSavingSig = signal(false);

    /**
     * جلب كل السيراميك
     */
    getAllCeramics(): Observable<CeramicsResponse> {
        this.isLoadingSig.set(true);
        return this.http.get<CeramicsResponse>(`${this.apiUrl}/Product/GetAllCeramics`)
            .pipe(
                tap(response => {
                    if (response.success && response.data?.ceramicList) {
                        const ceramics: Ceramic[] = response.data.ceramicList.map(c => ({
                            id: c.productId,
                            name: c.productName,
                            imageUrl: c.productImageUrl
                        }));
                        this.ceramicsSig.set(ceramics);
                    }
                    this.isLoadingSig.set(false);
                }),
                catchError(error => {
                    this.isLoadingSig.set(false);
                    console.error('Failed to load ceramics:', error);
                    return of({ success: false, message: '', data: { ceramicList: [] }, errors: {}, timestamp: '' });
                })
            );
    }

    // ============== Generation Methods ==============

    /**
     * توليد تصميم بلون فقط
     */
    generateWithPaintOnly(roomImage: File, colorCode: string): Observable<GenerateDesignResponse> {
        this.isGeneratingSig.set(true);

        const formData = new FormData();
        formData.append('roomImage', roomImage);

        return this.http.post<GenerateDesignResponse>(
            `${this.apiUrl}/Design/GenerateDesignWithPaintOnly`,
            formData,
            { params: { colorCode } }
        ).pipe(
            tap(response => {
                this.isGeneratingSig.set(false);
                if (response.success && response.data) {
                    this.generatedResultSig.set({
                        designId: response.data.designId,
                        originalImageUrl: URL.createObjectURL(roomImage),
                        generatedImageUrl: response.data.generatedImagePath,
                        isSaved: false,
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
     * توليد تصميم بسيراميك من المعرض
     */
    generateWithOurCeramic(roomImage: File, ceramicId: string): Observable<GenerateDesignResponse> {
        this.isGeneratingSig.set(true);

        const formData = new FormData();
        formData.append('imageFile', roomImage);

        return this.http.post<GenerateDesignResponse>(
            `${this.apiUrl}/Design/GenerateDesignWIthOurCeramic`,
            formData,
            { params: { ceramicId } }
        ).pipe(
            tap(response => {
                this.isGeneratingSig.set(false);
                if (response.success && response.data) {
                    this.generatedResultSig.set({
                        designId: response.data.designId,
                        originalImageUrl: URL.createObjectURL(roomImage),
                        generatedImageUrl: response.data.generatedImagePath,
                        isSaved: false,
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
     * توليد تصميم بسيراميك مخصص (صورة المستخدم)
     */
    generateWithUserCeramic(roomImage: File, ceramicImage: File): Observable<GenerateDesignResponse> {
        this.isGeneratingSig.set(true);

        const formData = new FormData();
        formData.append('roomImageFile', roomImage);
        formData.append('ceramicImageFile', ceramicImage);

        return this.http.post<GenerateDesignResponse>(
            `${this.apiUrl}/Design/GenerateDesignWithUserCeramicImage`,
            formData
        ).pipe(
            tap(response => {
                this.isGeneratingSig.set(false);
                if (response.success && response.data) {
                    this.generatedResultSig.set({
                        designId: response.data.designId,
                        originalImageUrl: URL.createObjectURL(roomImage),
                        generatedImageUrl: response.data.generatedImagePath,
                        isSaved: false,
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
     * توليد تصميم بسيراميك من المعرض + لون
     */
    generateWithOurCeramicAndPaint(roomImage: File, ceramicId: string, colorCode: string): Observable<GenerateDesignResponse> {
        this.isGeneratingSig.set(true);

        const formData = new FormData();
        formData.append('roomImageFile', roomImage);

        return this.http.post<GenerateDesignResponse>(
            `${this.apiUrl}/Design/GenerateDesignWithOurCeramicAndPaint`,
            formData,
            { params: { ceramicId, colorCode } }
        ).pipe(
            tap(response => {
                this.isGeneratingSig.set(false);
                if (response.success && response.data) {
                    this.generatedResultSig.set({
                        designId: response.data.designId,
                        originalImageUrl: URL.createObjectURL(roomImage),
                        generatedImageUrl: response.data.generatedImagePath,
                        isSaved: false,
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
     * توليد تصميم بسيراميك مخصص + لون
     */
    generateWithUserCeramicAndPaint(roomImage: File, ceramicImage: File, colorCode: string): Observable<GenerateDesignResponse> {
        this.isGeneratingSig.set(true);

        const formData = new FormData();
        formData.append('roomImageFile', roomImage);
        formData.append('ceramicImageFile', ceramicImage);

        return this.http.post<GenerateDesignResponse>(
            `${this.apiUrl}/Design/GenerateDesignWithUserCeramicAndPaint`,
            formData,
            { params: { colorCode } }
        ).pipe(
            tap(response => {
                this.isGeneratingSig.set(false);
                if (response.success && response.data) {
                    this.generatedResultSig.set({
                        designId: response.data.designId,
                        originalImageUrl: URL.createObjectURL(roomImage),
                        generatedImageUrl: response.data.generatedImagePath,
                        isSaved: false,
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

    // ============== Favorites Methods ==============

    /**
     * حفظ التصميم في المفضلة
     */
    saveDesign(designId: string): Observable<SaveDesignResponse> {
        this.isSavingSig.set(true);

        return this.http.post<SaveDesignResponse>(
            `${this.apiUrl}/Design/SaveDesign`,
            { designId }
        ).pipe(
            tap(response => {
                this.isSavingSig.set(false);
                if (response.success && response.data) {
                    // تحديث الـ generatedResult ليكون محفوظ
                    const current = this.generatedResultSig();
                    if (current && current.designId === designId) {
                        this.generatedResultSig.set({
                            ...current,
                            generatedImageUrl: response.data.generatedImageUrl,
                            isSaved: true
                        });
                    }
                }
            }),
            catchError(error => {
                this.isSavingSig.set(false);
                throw error;
            })
        );
    }

    /**
     * جلب كل التصاميم المحفوظة
     */
    getAllDesigns(): Observable<GetAllDesignsResponse> {
        return this.http.get<GetAllDesignsResponse>(`${this.apiUrl}/Design/GetAllDesigns`)
            .pipe(
                tap(response => {
                    if (response.success && response.data?.designsList) {
                        this.savedDesignsSig.set(response.data.designsList);
                    }
                }),
                catchError(error => {
                    console.error('Failed to load saved designs:', error);
                    return of({ success: false, message: '', data: { designsList: [] }, errors: {}, timestamp: '' });
                })
            );
    }

    /**
     * حذف تصميم من المفضلة
     */
    deleteDesign(designId: string): Observable<DeleteDesignResponse> {
        return this.http.delete<DeleteDesignResponse>(
            `${this.apiUrl}/Design/SoftDeleteDesign`,
            { params: { designId } }
        ).pipe(
            tap(response => {
                if (response.success) {
                    // حذف من القائمة المحلية
                    const current = this.savedDesignsSig();
                    this.savedDesignsSig.set(current.filter(d => d.designId !== designId));
                }
            }),
            catchError(error => {
                console.error('Failed to delete design:', error);
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
