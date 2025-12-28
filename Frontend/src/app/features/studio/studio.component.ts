import { Component, inject, signal, computed, effect, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { AuthService } from '../../Core/auth/auth.service';
import { DesignService } from '../../Core/services/design.service';
import { ThemeService } from '../../Core/services/theme.service';
import { LanguageService } from '../../Core/services/language.service';
import { ProgressMessagesService } from '../../Core/services/progress-messages.service';
import { Ceramic, GeneratedDesign, SavedDesign } from '../../shared/models/design.model';

// المكونات الفرعية
import { ImageUploadComponent } from './components/image-upload/image-upload.component';
import { GalleryGridComponent } from './components/gallery-grid/gallery-grid.component';
import { PreviewPanelComponent } from './components/preview-panel/preview-panel.component';
import { ThemeToggleComponent } from '../../shared/components/theme-toggle.component';
import { LanguageSwitcherComponent } from '../../shared/components/language-switcher.component';

@Component({
    selector: 'app-studio',
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        TranslatePipe,
        ImageUploadComponent,
        GalleryGridComponent,
        PreviewPanelComponent,
        ThemeToggleComponent,
        LanguageSwitcherComponent
    ],
    templateUrl: './studio.component.html',
    styleUrl: './studio.component.css',
})
export class StudioComponent implements OnInit {
    private authService = inject(AuthService);
    private designService = inject(DesignService);
    readonly themeService = inject(ThemeService);
    readonly languageService = inject(LanguageService);
    readonly progressService = inject(ProgressMessagesService);
    private router = inject(Router);

    constructor() {
        // مراقبة حالة التوليد لإدارة الرسائل
        effect(() => {
            if (this.isGenerating()) {
                this.progressService.startMessages(1); // Ceramic type for messages
            } else {
                this.progressService.stopMessages();
            }
        });

        // بدء عداد انتهاء صلاحية زر الحفظ عند التوليد
        effect(() => {
            const result = this.generatedResult();
            if (result && !result.isSaved) {
                this.startExpiryTimer();
            }
        });
    }

    // حالة المستخدم
    currentUser = this.authService.currentUserSig;

    // حالة التصميم
    roomImage = signal<File | null>(null);
    roomImageUrl = signal<string | null>(null);

    // اختيارات السيراميك
    useCeramic = signal(true);
    ceramicSource = signal<'gallery' | 'custom'>('gallery');
    selectedCeramic = signal<Ceramic | null>(null);
    customCeramicImage = signal<File | null>(null);

    // اختيارات اللون
    usePaint = signal(false);
    selectedColor = signal('#E44C51');

    // حالة التوليد والحفظ
    isGenerating = this.designService.isGeneratingSig;
    isSaving = this.designService.isSavingSig;
    generatedResult = this.designService.generatedResultSig;

    // التصاميم المحفوظة
    savedDesigns = this.designService.savedDesignsSig;

    // حالة Modal الحذف
    showDeleteModal = signal(false);
    designToDelete = signal<string | null>(null);
    isDeleting = signal(false);

    // حالة Modal النجاح
    showSuccessModal = signal(false);

    // حالة Modal عرض الصورة
    showImageViewer = signal(false);
    viewingImageUrl = signal<string | null>(null);

    // عداد الـ 4 دقائق
    saveButtonExpired = signal(false);
    private expiryTimer: any = null;

    ngOnInit(): void {
        // جلب التصاميم المحفوظة عند التحميل
        this.loadSavedDesigns();
    }

    loadSavedDesigns(): void {
        this.designService.getAllDesigns().subscribe();
    }

    // بدء عداد الـ 4 دقائق
    private startExpiryTimer(): void {
        // إلغاء أي عداد سابق
        if (this.expiryTimer) {
            clearTimeout(this.expiryTimer);
        }
        this.saveButtonExpired.set(false);

        // 4 دقائق = 240000 مللي ثانية
        this.expiryTimer = setTimeout(() => {
            this.saveButtonExpired.set(true);
        }, 240000);
    }

    // التحقق من جاهزية التوليد
    canGenerate = computed(() => {
        if (!this.roomImage() || this.isGenerating()) return false;

        const hasCeramic = this.useCeramic() &&
            (this.ceramicSource() === 'gallery' ? this.selectedCeramic() !== null : this.customCeramicImage() !== null);
        const hasPaint = this.usePaint();

        return hasCeramic || hasPaint;
    });

    // معالجة رفع صورة الغرفة
    onRoomImageSelected(file: File): void {
        const oldUrl = this.roomImageUrl();
        if (oldUrl) {
            URL.revokeObjectURL(oldUrl);
        }

        this.roomImage.set(file);
        this.roomImageUrl.set(URL.createObjectURL(file));
        this.designService.clearResult();
    }

    // تبديل استخدام السيراميك
    toggleCeramic(): void {
        this.useCeramic.update(v => !v);
        if (!this.useCeramic()) {
            this.selectedCeramic.set(null);
            this.customCeramicImage.set(null);
        }
    }

    // تبديل استخدام اللون
    togglePaint(): void {
        this.usePaint.update(v => !v);
    }

    // تغيير مصدر السيراميك
    setCeramicSource(source: 'gallery' | 'custom'): void {
        this.ceramicSource.set(source);
        this.selectedCeramic.set(null);
        this.customCeramicImage.set(null);
    }

    // معالجة اختيار سيراميك من المعرض
    onCeramicSelected(ceramic: Ceramic): void {
        this.selectedCeramic.set(ceramic);
        this.customCeramicImage.set(null);
    }

    // معالجة رفع صورة سيراميك مخصصة
    onCustomCeramicSelected(file: File): void {
        this.customCeramicImage.set(file);
        this.selectedCeramic.set(null);
    }

    // معالجة رفع صورة سيراميك من input
    onCustomCeramicUpload(event: Event): void {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.onCustomCeramicSelected(input.files[0]);
        }
    }

    // تغيير اللون
    onColorChange(event: Event): void {
        const input = event.target as HTMLInputElement;
        this.selectedColor.set(input.value);
    }

    // توليد التصميم
    generate(): void {
        const roomImage = this.roomImage();
        if (!roomImage) return;

        const useCeramic = this.useCeramic();
        const usePaint = this.usePaint();
        const colorCode = this.selectedColor().replace('#', '');

        if (useCeramic && usePaint) {
            // سيراميك + لون
            if (this.ceramicSource() === 'gallery' && this.selectedCeramic()) {
                this.designService.generateWithOurCeramicAndPaint(
                    roomImage,
                    this.selectedCeramic()!.id,
                    colorCode
                ).subscribe({ error: this.handleError });
            } else if (this.customCeramicImage()) {
                this.designService.generateWithUserCeramicAndPaint(
                    roomImage,
                    this.customCeramicImage()!,
                    colorCode
                ).subscribe({ error: this.handleError });
            }
        } else if (useCeramic) {
            // سيراميك فقط
            if (this.ceramicSource() === 'gallery' && this.selectedCeramic()) {
                this.designService.generateWithOurCeramic(
                    roomImage,
                    this.selectedCeramic()!.id
                ).subscribe({ error: this.handleError });
            } else if (this.customCeramicImage()) {
                this.designService.generateWithUserCeramic(
                    roomImage,
                    this.customCeramicImage()!
                ).subscribe({ error: this.handleError });
            }
        } else if (usePaint) {
            // لون فقط
            this.designService.generateWithPaintOnly(
                roomImage,
                colorCode
            ).subscribe({ error: this.handleError });
        }
    }

    private handleError = (err: any) => {
        console.error('Generation error:', err);
        alert(err.error?.message || 'حدث خطأ أثناء التوليد');
    };

    // حفظ في المفضلة
    saveToFavorites(): void {
        const result = this.generatedResult();
        if (result && !result.isSaved && !this.saveButtonExpired()) {
            this.designService.saveDesign(result.designId).subscribe({
                next: () => {
                    // إلغاء عداد الانتهاء
                    if (this.expiryTimer) {
                        clearTimeout(this.expiryTimer);
                    }
                    // عرض modal النجاح
                    this.showSuccessModal.set(true);
                    // إعادة تحميل المفضلة
                    this.loadSavedDesigns();
                },
                error: (err) => {
                    alert(err.error?.message || 'حدث خطأ أثناء الحفظ');
                }
            });
        }
    }

    // إغلاق modal النجاح
    closeSuccessModal(): void {
        this.showSuccessModal.set(false);
    }

    // فتح عارض الصور
    openImageViewer(imageUrl: string): void {
        this.viewingImageUrl.set(imageUrl);
        this.showImageViewer.set(true);
    }

    // إغلاق عارض الصور
    closeImageViewer(): void {
        this.showImageViewer.set(false);
        this.viewingImageUrl.set(null);
    }

    // تحميل النتيجة
    async downloadResult(): Promise<void> {
        const result = this.generatedResult();
        if (result) {
            try {
                const response = await fetch(result.generatedImageUrl);
                const blob = await response.blob();
                const blobUrl = URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = blobUrl;
                link.download = `shatbli-design-${Date.now()}.png`;
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
                URL.revokeObjectURL(blobUrl);
            } catch (error) {
                window.open(result.generatedImageUrl, '_blank');
            }
        }
    }

    // مشاركة النتيجة
    shareResult(): void {
        const result = this.generatedResult();
        if (result && navigator.share) {
            navigator.share({
                title: 'تصميمي من شطبلي',
                text: 'شاهد تصميم غرفتي الجديد!',
                url: result.generatedImageUrl
            }).catch(() => { });
        } else {
            navigator.clipboard.writeText(result?.generatedImageUrl || '');
            alert('تم نسخ رابط الصورة!');
        }
    }

    // تسجيل الخروج
    logout(): void {
        this.authService.logout();
        this.router.navigate(['/auth/login']);
    }

    // ============== Delete Modal Methods ==============

    // فتح modal الحذف
    openDeleteModal(designId: string): void {
        this.designToDelete.set(designId);
        this.showDeleteModal.set(true);
    }

    // إغلاق modal الحذف
    closeDeleteModal(): void {
        this.showDeleteModal.set(false);
        this.designToDelete.set(null);
    }

    // تأكيد الحذف
    confirmDelete(): void {
        const designId = this.designToDelete();
        if (!designId) return;

        this.isDeleting.set(true);
        this.designService.deleteDesign(designId).subscribe({
            next: () => {
                this.isDeleting.set(false);
                this.closeDeleteModal();
            },
            error: (err) => {
                this.isDeleting.set(false);
                alert(err.error?.message || 'حدث خطأ أثناء الحذف');
            }
        });
    }
}
