import { Component, inject, signal, computed, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { AuthService } from '../../Core/auth/auth.service';
import { DesignService } from '../../Core/services/design.service';
import { ThemeService } from '../../Core/services/theme.service';
import { LanguageService } from '../../Core/services/language.service';
import { ProgressMessagesService } from '../../Core/services/progress-messages.service';
import { Ceramic, DesignType, GeneratedDesign } from '../../shared/models/design.model';

// المكونات الفرعية
import { ImageUploadComponent } from './components/image-upload/image-upload.component';
import { DesignSelectorComponent } from './components/design-selector/design-selector.component';
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
        DesignSelectorComponent,
        GalleryGridComponent,
        PreviewPanelComponent,
        ThemeToggleComponent,
        LanguageSwitcherComponent
    ],
    templateUrl: './studio.component.html',
    styleUrl: './studio.component.css',
})
export class StudioComponent {
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
                this.progressService.startMessages(this.designType());
            } else {
                this.progressService.stopMessages();
            }
        });
    }

    // حالة المستخدم
    currentUser = this.authService.currentUserSig;

    // حالة التصميم
    roomImage = signal<File | null>(null);
    roomImageUrl = signal<string | null>(null);
    designType = signal<DesignType>(DesignType.Ceramic);
    selectedCeramic = signal<Ceramic | null>(null);
    customImage = signal<File | null>(null);

    // حالة التوليد
    isGenerating = this.designService.isGeneratingSig;
    generatedResult = this.designService.generatedResultSig;

    // التحقق من جاهزية التوليد
    canGenerate = computed(() => {
        return this.roomImage() !== null &&
            (this.selectedCeramic() !== null || this.customImage() !== null) &&
            !this.isGenerating();
    });

    // معالجة رفع صورة الغرفة
    onRoomImageSelected(file: File): void {
        // تحرير الـ URL القديم لمنع memory leak
        const oldUrl = this.roomImageUrl();
        if (oldUrl) {
            URL.revokeObjectURL(oldUrl);
        }

        this.roomImage.set(file);
        this.roomImageUrl.set(URL.createObjectURL(file));
        this.designService.clearResult();
    }

    // معالجة تغيير نوع التصميم
    onDesignTypeChanged(type: DesignType): void {
        this.designType.set(type);
        this.selectedCeramic.set(null);
        this.customImage.set(null);
    }

    // معالجة اختيار تصميم من المعرض
    onDesignSelected(ceramic: Ceramic): void {
        this.selectedCeramic.set(ceramic);
        this.customImage.set(null);
    }

    // معالجة رفع صورة مخصصة
    onCustomImageSelected(file: File): void {
        this.customImage.set(file);
        this.selectedCeramic.set(null);
    }

    // توليد التصميم
    generate(): void {
        const roomImage = this.roomImage();
        if (!roomImage) return;

        const customImage = this.customImage();
        const selectedCeramic = this.selectedCeramic();

        if (customImage) {
            // توليد بصورة مخصصة
            this.designService.generateDesignWithUserImage(
                roomImage,
                customImage,
                this.designType()
            ).subscribe({
                error: (err) => {
                    alert(err.error?.message || 'حدث خطأ أثناء التوليد');
                }
            });
        } else if (selectedCeramic) {
            // توليد من المعرض
            this.designService.generateDesign(
                roomImage,
                selectedCeramic.id,
                this.designType()
            ).subscribe({
                error: (err) => {
                    alert(err.error?.message || 'حدث خطأ أثناء التوليد');
                }
            });
        }
    }

    // تحميل النتيجة
    async downloadResult(): Promise<void> {
        const result = this.generatedResult();
        if (result) {
            try {
                // جلب الصورة كـ blob لتجاوز قيود CORS
                const response = await fetch(result.generatedImageUrl);
                const blob = await response.blob();

                // إنشاء رابط تحميل من الـ blob
                const blobUrl = URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = blobUrl;
                link.download = `shatbli-design-${Date.now()}.png`;
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);

                // تحرير الـ URL
                URL.revokeObjectURL(blobUrl);
            } catch (error) {
                // في حالة فشل fetch، فتح الصورة في تاب جديد
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
            // نسخ الرابط
            navigator.clipboard.writeText(result?.generatedImageUrl || '');
            alert('تم نسخ رابط الصورة!');
        }
    }

    // تسجيل الخروج
    logout(): void {
        this.authService.logout();
        this.router.navigate(['/auth/login']);
    }
}
