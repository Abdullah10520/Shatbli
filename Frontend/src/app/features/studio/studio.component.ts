import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../Core/auth/auth.service';
import { DesignService } from '../../Core/services/design.service';
import { Design, DesignType, GeneratedDesign } from '../../shared/models/design.model';

// المكونات الفرعية
import { ImageUploadComponent } from './components/image-upload/image-upload.component';
import { DesignSelectorComponent } from './components/design-selector/design-selector.component';
import { GalleryGridComponent } from './components/gallery-grid/gallery-grid.component';
import { PreviewPanelComponent } from './components/preview-panel/preview-panel.component';

@Component({
    selector: 'app-studio',
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        ImageUploadComponent,
        DesignSelectorComponent,
        GalleryGridComponent,
        PreviewPanelComponent
    ],
    templateUrl: './studio.component.html',
    styleUrl: './studio.component.css',
})
export class StudioComponent {
    private authService = inject(AuthService);
    private designService = inject(DesignService);
    private router = inject(Router);

    // حالة المستخدم
    currentUser = this.authService.currentUserSig;

    // حالة التصميم
    roomImage = signal<File | null>(null);
    roomImageUrl = signal<string | null>(null);
    designType = signal<DesignType>(DesignType.Ceramic);
    selectedDesign = signal<Design | null>(null);
    customImage = signal<File | null>(null);

    // حالة التوليد
    isGenerating = this.designService.isGeneratingSig;
    generatedResult = this.designService.generatedResultSig;

    // التحقق من جاهزية التوليد
    canGenerate = computed(() => {
        return this.roomImage() !== null &&
            (this.selectedDesign() !== null || this.customImage() !== null) &&
            !this.isGenerating();
    });

    // معالجة رفع صورة الغرفة
    onRoomImageSelected(file: File): void {
        this.roomImage.set(file);
        this.roomImageUrl.set(URL.createObjectURL(file));
        this.designService.clearResult();
    }

    // معالجة تغيير نوع التصميم
    onDesignTypeChanged(type: DesignType): void {
        this.designType.set(type);
        this.selectedDesign.set(null);
        this.customImage.set(null);
    }

    // معالجة اختيار تصميم من المعرض
    onDesignSelected(design: Design): void {
        this.selectedDesign.set(design);
        this.customImage.set(null);
    }

    // معالجة رفع صورة مخصصة
    onCustomImageSelected(file: File): void {
        this.customImage.set(file);
        this.selectedDesign.set(null);
    }

    // توليد التصميم
    generate(): void {
        const roomImage = this.roomImage();
        if (!roomImage) return;

        const customImage = this.customImage();
        const selectedDesign = this.selectedDesign();

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
        } else if (selectedDesign) {
            // توليد من المعرض
            this.designService.generateDesign(
                roomImage,
                selectedDesign.id,
                this.designType()
            ).subscribe({
                error: (err) => {
                    alert(err.error?.message || 'حدث خطأ أثناء التوليد');
                }
            });
        }
    }

    // تحميل النتيجة
    downloadResult(): void {
        const result = this.generatedResult();
        if (result) {
            const link = document.createElement('a');
            link.href = result.generatedImageUrl;
            link.download = `shatbli-design-${Date.now()}.jpg`;
            link.click();
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
