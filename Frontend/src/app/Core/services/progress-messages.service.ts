import { Injectable, signal, inject, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { DesignType } from '../../shared/models/design.model';

export interface ProgressMessage {
    key: string;
    text: string;
}

@Injectable({
    providedIn: 'root'
})
export class ProgressMessagesService implements OnDestroy {
    private readonly translateService = inject(TranslateService);

    // الرسالة الحالية
    readonly currentMessage = signal<string>('');

    // مؤشر الرسالة الحالية
    private messageIndex = 0;

    // الـ interval
    private intervalId: any = null;

    // مفاتيح الرسائل للسيراميك
    private readonly ceramicKeys = [
        'progress.received',
        'progress.analyzing',
        'progress.preparing_ceramic',
        'progress.ai_working',
        'progress.applying_design',
        'progress.final_touches',
        'progress.almost_done'
    ];

    // مفاتيح الرسائل للدهان
    private readonly paintKeys = [
        'progress.received',
        'progress.analyzing',
        'progress.preparing_paint',
        'progress.ai_working',
        'progress.applying_design',
        'progress.final_touches',
        'progress.almost_done'
    ];

    // مفاتيح الرسائل للسيراميك + دهان
    private readonly combinedKeys = [
        'progress.received',
        'progress.analyzing',
        'progress.working_ceramic',
        'progress.working_paint',
        'progress.combining',
        'progress.ai_working',
        'progress.applying_design',
        'progress.final_touches',
        'progress.almost_done'
    ];

    /**
     * بدء عرض الرسائل
     */
    startMessages(designType: DesignType, isCombined: boolean = false): void {
        this.stopMessages();
        this.messageIndex = 0;

        // اختيار مفاتيح الرسائل المناسبة
        let keys: string[];
        if (isCombined) {
            keys = this.combinedKeys;
        } else if (designType === DesignType.Ceramic) {
            keys = this.ceramicKeys;
        } else {
            keys = this.paintKeys;
        }

        // عرض الرسالة الأولى
        this.showMessage(keys[0]);

        // تغيير الرسالة كل 3 ثوانٍ
        this.intervalId = setInterval(() => {
            this.messageIndex++;
            if (this.messageIndex < keys.length) {
                this.showMessage(keys[this.messageIndex]);
            } else {
                // إعادة من الرسالة الرابعة (ai_working)
                this.messageIndex = 3;
                this.showMessage(keys[this.messageIndex]);
            }
        }, 3000);
    }

    /**
     * عرض رسالة محددة
     */
    private showMessage(key: string): void {
        this.translateService.get(key).subscribe(text => {
            this.currentMessage.set(text);
        });
    }

    /**
     * إيقاف عرض الرسائل
     */
    stopMessages(): void {
        if (this.intervalId) {
            clearInterval(this.intervalId);
            this.intervalId = null;
        }
        this.currentMessage.set('');
        this.messageIndex = 0;
    }

    ngOnDestroy(): void {
        this.stopMessages();
    }
}
